﻿namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Sitecore.Data.Items;
    using Unic.Flex.DomainModel.Forms;

    public interface IContextService
    {
        IForm LoadForm(string dataSource, ISitecoreContext sitecoreContext);

        string GetRenderingDatasource(Item item, DeviceItem device);
    }
}
