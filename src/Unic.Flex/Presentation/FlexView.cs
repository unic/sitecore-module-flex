namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.Model;
    using Unic.Flex.Model.Forms;

    public abstract class FlexView<TViewModel, TDomainModel> : WebViewPage<TViewModel> where TViewModel : IViewModel where TDomainModel : ItemBase
    {
        protected virtual TDomainModel DomainModel 
        {
            get
            {
                return Model.DomainModel as TDomainModel;
            }
        }
    }
}
