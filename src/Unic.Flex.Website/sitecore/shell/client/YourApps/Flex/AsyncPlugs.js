var globalAppRef;

define(["sitecore"], function (Sitecore) {
  var AsyncPlugs = Sitecore.Definitions.App.extend({
      initialized: function () {

          globalAppRef = this;

          this.TasksListControl.on("change:sorting", this.sorting, this);

          this.refresh();
      },

      sorting: function () {

          this.refresh();
      },

      refresh: function () {

          var sorting = this.TasksListControl.get('sorting');
          var sortField = sorting != null && sorting != "" ? sorting.substring(1) : "";
          var sortDirection = sorting != null && sorting != "" && sorting.substring(0, 1) == "a" ? "asc" : "desc";

          var params = $.param({ 'sort': sortField, 'direction': sortDirection });
          var apiUrl = "/api/sitecore/FlexSpeak/GetAsyncTasks?" + params;
          var requestOptions = { parameters: "", onSuccess: null, url: apiUrl };
          this.Tasks.viewModel.getData(requestOptions);
      },

      resetTask: function (id) {

          $.ajax({
              url: "/api/sitecore/FlexSpeak/ResetAsyncTask",
              data: { id: id },
              success : function() {
                  globalAppRef.refresh();
              },
              error : function() {
                  alert('An error occurred!');
              }
          });
      },

      deleteTask: function (id) {

          if (!confirm('Are you sure you want to delete this task?')) return false;

          $.ajax({
              url: "/api/sitecore/FlexSpeak/DeleteAsyncTask",
              data: { id: id },
              success: function () {
                  globalAppRef.refresh();
              },
              error: function () {
                  alert('An error occurred!');
              }
          });
      }
  });

  return AsyncPlugs;
});