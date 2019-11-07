namespace Unic.Flex.Core.Mapping
{
    using System;
    using Glass.Mapper;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Web;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Repository containg data access for the forms.
    /// </summary>
    public class FormRepository : IFormRepository
    {
        /// <summary>
        /// The sitecore context
        /// </summary>
        private readonly IRequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormRepository"/> class.
        /// </summary>
        /// <param name="requestContext">The sitecore context.</param>
        public FormRepository(IRequestContext requestContext)
        {
            this.requestContext = requestContext;
        }

        /// <summary>
        /// Loads a form based on the data source from Sitecore.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="useVersionCountDisabler">if set to <c>true</c> the version count disabler is used to load the form.</param>
        /// <returns>
        /// The loaded form domain model
        /// </returns>
        public virtual IForm LoadForm(string dataSource, bool useVersionCountDisabler = false)
        {
            Assert.ArgumentCondition(Sitecore.Data.ID.IsID(dataSource), dataSource, "Datasource is not valid");
            var id = Guid.Parse(dataSource);

            return this.LoadItem<IForm>(id, useVersionCountDisabler, isLazy: true, infer: true);
        }

        /// <summary>
        /// Loads an item from the data source.
        /// </summary>
        /// <typeparam name="T">Type of the item to load</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Item if found
        /// </returns>
        public T LoadItem<T>(Guid id) where T : class
        {
            return LoadItem<T>(id, useVersionCountDisabler: false);
        }

        /// <summary>
        /// Loads an item from the data source.
        /// </summary>
        /// <typeparam name="T">Type of the item to load</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="useVersionCountDisabler">Disable Version Count</param>
        /// <returns>
        /// Item if found
        /// </returns>
        public T LoadItem<T>(Guid id, bool useVersionCountDisabler) where T : class
        {
            return LoadItem<T>(id, useVersionCountDisabler, isLazy: true);
        }

        /// <summary>
        /// Get Item Options
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <param name="useVersionCountDisabler"></param>
        /// <param name="isLazy"></param>
        /// <returns></returns>
        private T LoadItem<T>(Guid id, bool useVersionCountDisabler, bool isLazy) where T : class
        {
            //Set isLazy and infer to be the same value, in order to prevent possible IIS Application Pool crashes. 
            //ref: https://sitecorefootsteps.blogspot.com/2019/07/glass-mapper-stackoverflow-memory.html
            
            return LoadItem<T>(id, useVersionCountDisabler, isLazy, infer: isLazy);
        }

        /// <summary>
        /// Get Item Options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="useVersionCountDisabler"></param>
        /// <param name="isLazy"></param>
        /// <param name="infer"></param>
        /// <returns></returns>
        private T LoadItem<T>(Guid id, bool useVersionCountDisabler, bool isLazy, bool infer) where T : class
        {
            var options = GetOptions(id, useVersionCountDisabler, isLazy, infer);

            return this.requestContext.SitecoreService.GetItem<T>(options);
        }

        /// <summary>
        /// Get Item Options
        /// </summary>
        /// <param name="id"></param>
        /// <param name="useVersionCountDisabler"></param>
        /// <param name="lazyLoad"></param>
        /// <param name="infer"></param>
        /// <returns></returns>
        private static GetItemByIdOptions GetOptions(Guid id, bool useVersionCountDisabler, bool lazyLoad = true,
            bool infer = false)
        {
            return new GetItemByIdOptions(id)
            {
                VersionCount = !useVersionCountDisabler,
                Lazy = lazyLoad ? LazyLoading.Enabled : LazyLoading.Disabled,
                InferType = infer
            };
        }
    }
}
