namespace Unic.Flex.Website.Controllers
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Newtonsoft.Json;
    using Unic.Flex.Core.Plugs;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Website.Models.FlexSpeak;

    /// <summary>
    /// Mvc controller for SPEAK application.
    /// </summary>
    public class FlexSpeakController : Controller
    {
        /// <summary>
        /// The task service
        /// </summary>
        private readonly ITaskService taskService;

        /// <summary>
        /// The sitecore context
        /// </summary>
        private readonly ISitecoreContext sitecoreContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexSpeakController"/> class.
        /// </summary>
        public FlexSpeakController()
        {
            this.taskService = Core.DependencyInjection.DependencyResolver.Resolve<ITaskService>();
            this.sitecoreContext = Core.DependencyInjection.DependencyResolver.Resolve<ISitecoreContext>();
        }

        /// <summary>
        /// Gets the asynchronous tasks.
        /// </summary>
        /// <returns>Json with all asynchronous tasks saved in the database.</returns>
        public ActionResult GetAsyncTasks()
        {
            var tasks = new AsyncTaks();

            foreach (var job in this.taskService.GetAllJobs())
            {
                var form = this.sitecoreContext.GetItem<ItemBase>(job.ItemId);
                var formName = form != null ? form.ItemName : "-";

                foreach (var task in job.Tasks)
                {
                    var plug = this.sitecoreContext.GetItem<ItemBase>(task.ItemId);
                    var plugName = plug != null ? plug.ItemName : "-";
                    
                    tasks.Data.Add(new AsyncTask
                                       {
                                           TaskId = task.Id,
                                           Form = formName,
                                           Plug = plugName,
                                           Attemps = task.NumberOfTries,
                                           LastTry = task.LastTry
                                       });
                }
            }

            this.Response.ContentType = "application/json";
            return this.Content(JsonConvert.SerializeObject(tasks));
        }
    }
}