var globalAppRef;

define(["sitecore"], function (Sitecore) {
  var AsyncPlugs = Sitecore.Definitions.App.extend({
      initialized: function () {

          globalAppRef = this;

          this.refresh();
      },

      refresh: function () {

          var apiUrl = "/api/sitecore/FlexSpeak/GetAsyncTasks";
          var requestOptions = { parameters: "", onSuccess: null, url: apiUrl };
          this.Tasks.viewModel.getData(requestOptions);
      },

      reset: function (id) {

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
      }
  });

  return AsyncPlugs;
});