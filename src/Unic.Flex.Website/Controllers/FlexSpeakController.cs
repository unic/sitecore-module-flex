namespace Unic.Flex.Website.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Newtonsoft.Json;
    using Sitecore.Data.Managers;
    using Sitecore.Globalization;
    using Unic.Flex.Core.Authorization;
    using Unic.Flex.Core.Plugs;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Website.Models.FlexSpeak.GetAsyncTasks;
    using Unic.Flex.Website.Models.FlexSpeak.GetAvailableForms;

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

        /// <summary>
        /// Gets all available forms in the different repositories.
        /// </summary>
        /// <returns>Json result</returns>
        [AdministratorOnly]
        public ActionResult GetAvailableForms()
        {
            var rows = new List<DataRow>();

            foreach (var language in LanguageManager.GetLanguages(this.sitecoreContext.Database))
            {
                using (new LanguageSwitcher(language))
                {
                    var allForms = this.sitecoreContext.Query<StatisticForm>("fast://*[@@templateid='{3AFE4256-1C3E-4441-98AF-B3D0037A8B1F}']");
                    foreach (var row in allForms.Where(f => f.Repository != null).GroupBy(f => f.Repository.FullPath))
                    {
                        rows.Add(new DataRow { Repository = row.Key, Forms = row.Count(), Language = language.Name });
                    }
                }
            }

            // generate object
            var data = new MainWrapper { Data = new DataWrapper { DataSet = new[] { new DataSet { Rows = rows } } } };

            // create the response
            this.Response.ContentType = "application/json";
            return this.Content(JsonConvert.SerializeObject(data));
        }
    }
}