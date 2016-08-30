define(["sitecore"], function (Sitecore) {
  var Overview = Sitecore.Definitions.App.extend({
      initialized: function () {

          var apiUrl = "/flexapi/FlexSpeak/GetAvailableForms";
          var requestOptions = { parameters: "", onSuccess: null, url: apiUrl };
          this.AvailableFormsChartData.viewModel.getData(requestOptions);
      }
  });

  return Overview;
});