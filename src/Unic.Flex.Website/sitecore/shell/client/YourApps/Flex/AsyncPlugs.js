define(["sitecore"], function (Sitecore) {
  var AsyncPlugs = Sitecore.Definitions.App.extend({
      initialized: function () {

          this.refresh();
      },

      refresh: function () {

          var apiUrl = "/api/sitecore/FlexSpeak/GetAsyncTasks";
          var requestOptions = { parameters: "", onSuccess: null, url: apiUrl };
          this.Tasks.viewModel.getData(requestOptions);
      }
  });

  return AsyncPlugs;
});