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
    using Unic.Flex.Model;
    using Unic.Flex.Model.Forms;
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
        /// Deletes the asynchronous task from the database.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>JSON with boolean value</returns>
        [AdministratorOnly]
        public ActionResult DeleteAsyncTask(int id)
        {
            return this.Json(this.taskService.DeleteTaskById(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets all available forms in the different repositories.
        /// </summary>
        /// <returns>Json result</returns>
        [AdministratorOnly]
        public ActionResult GetAvailableForms()
        {
            var rows = new List<DataRow>();

            // add forms for each language
            var allLanguages = LanguageManager.GetLanguages(this.sitecoreContext.Database);
            foreach (var language in allLanguages)
            {
                using (new LanguageSwitcher(language))
                {
                    this.AddForms(rows, language.Name);
                }
            }

            // add empty rows for repository without form
            foreach (var repository in rows.GroupBy(r => r.Repository))
            {
                foreach (var language in allLanguages)
                {
                    if (!repository.Any(r => r.Language.Equals(language.Name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        rows.Add(new DataRow { Repository = repository.Key, Forms = 0, Language = language.Name.ToLowerInvariant() });
                    }
                }
            }

            // add total forms
            using (new VersionCountDisabler())
            {
                this.AddForms(rows, "total");
            }

            // generate object
            var sortedRows = rows.OrderBy(r => r.Repository).ThenBy(r => r.Language);
            var data = new MainWrapper { Data = new DataWrapper { DataSet = new[] { new DataSet { Rows = sortedRows } } } };

            // create the response
            this.Response.ContentType = "application/json";
            return this.Content(JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// Adds the forms to the rows collection.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <param name="languageName">Name of the language.</param>
        private void AddForms(List<DataRow> rows, string languageName)
        {
            var allForms = this.sitecoreContext.Query<StatisticForm>("fast://*[@@templateid='{3AFE4256-1C3E-4441-98AF-B3D0037A8B1F}']");
            foreach (var row in allForms.Where(f => f.Repository != null).GroupBy(f => f.Repository.FullPath))
            {
                rows.Add(new DataRow { Repository = row.Key, Forms = row.Count(), Language = languageName.ToLowerInvariant() });
            }
        }
    }
}