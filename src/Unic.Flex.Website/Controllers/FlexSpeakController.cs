namespace Unic.Flex.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Newtonsoft.Json;
    using Unic.Flex.Core.Authorization;
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
        /// <param name="sort">The sort.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>
        /// Json with all asynchronous tasks saved in the database.
        /// </returns>
        [AdministratorOnly]
        public ActionResult GetAsyncTasks(string sort, string direction)
        {
            // initialize
            var taskList = new List<AsyncTask>();
            
            // get elements
            foreach (var job in this.taskService.GetAllJobs())
            {
                var form = this.sitecoreContext.GetItem<ItemBase>(job.ItemId);
                var formName = form != null ? form.ItemName : "-";

                foreach (var task in job.Tasks)
                {
                    var plug = this.sitecoreContext.GetItem<ItemBase>(task.ItemId);
                    var plugName = plug != null ? plug.ItemName : "-";

                    taskList.Add(new AsyncTask
                                       {
                                           TaskId = task.Id,
                                           Form = formName,
                                           Plug = plugName,
                                           Attemps = task.NumberOfTries,
                                           LastTry = task.LastTry
                                       });
                }
            }

            // sort
            Func<AsyncTask, object> sortPredicate;
            switch (sort)
            {
                case "form":
                    sortPredicate = x => x.Form;
                    break;

                case "plug":
                    sortPredicate = x => x.Plug;
                    break;

                case "attempts":
                    sortPredicate = x => x.Attemps;
                    break;

                case "last_try":
                    sortPredicate = x => x.LastTry;
                    break;
                
                default:
                    sortPredicate = x => x.TaskId;
                    break;
            }

            // generate object
            var tasks = new AsyncTaks
                            {
                                Data = direction == "asc"
                                        ? taskList.OrderBy(sortPredicate)
                                        : taskList.OrderByDescending(sortPredicate)
                            };

            // create the response
            this.Response.ContentType = "application/json";
            return this.Content(JsonConvert.SerializeObject(tasks));
        }

        /// <summary>
        /// Resets the asynchronous task retry counter.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>JSON with boolean value</returns>
        [AdministratorOnly]
        public ActionResult ResetAsyncTask(int id)
        {
            return this.Json(this.taskService.ResetTaskById(id), JsonRequestBehavior.AllowGet);
        }
    }
}