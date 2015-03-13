namespace Unic.Flex.Core.Plugs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Mail;
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Sites;
    using Unic.Configuration;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.Database;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Entities;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Plugs;
    using Form = Unic.Flex.Model.Entities.Form;

    /// <summary>
    /// Task service for executing plugs asynchronous.
    /// </summary>
    public class TaskService : ITaskService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The context service
        /// </summary>
        private readonly IContextService contextService;

        /// <summary>
        /// The user data repository
        /// </summary>
        private readonly IUserDataRepository userDataRepository;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// The dictionary repository
        /// </summary>
        private readonly IDictionaryRepository dictionaryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="contextService">The context service.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="dictionaryRepository">The dictionary repository.</param>
        public TaskService(IUnitOfWork unitOfWork, IContextService contextService, IUserDataRepository userDataRepository, ILogger logger, IConfigurationManager configurationManager, IDictionaryRepository dictionaryRepository)
        {
            this.unitOfWork = unitOfWork;
            this.contextService = contextService;
            this.userDataRepository = userDataRepository;
            this.logger = logger;
            this.configurationManager = configurationManager;
            this.dictionaryRepository = dictionaryRepository;
        }

        /// <summary>
        /// Executes all jobs.
        /// </summary>
        /// <param name="site">The site.</param>
        public virtual void ExecuteAll(SiteContext site)
        {
            this.logger.Debug("Execute all jobs from database", this);
            var jobs = this.GetAllJobs();
            foreach (var job in jobs)
            {
                this.Execute(job, site);
            }
        }

        /// <summary>
        /// Executes the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="site">The site.</param>
        public virtual void Execute(Job job, SiteContext site)
        {
            Assert.ArgumentNotNull(job, "job");
            Assert.ArgumentNotNull(site, "site");

            // log
            this.logger.Debug(string.Format("Initialize executing of job '{0}'", job.Id), this);

            // get config values
            var maxRetries = this.configurationManager.Get<GlobalConfiguration>(c => c.MaxRetries);
            var timeBetweenTries = this.configurationManager.Get<GlobalConfiguration>(c => c.TimeBetweenTries);

            // get the original language
            var language = Sitecore.Context.Language;

            // start thread and execute specific job
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                using (new SiteContextSwitcher(site))
                {
                    using (new LanguageSwitcher(language))
                    {
                        this.ExecuteJob(site, language, job, maxRetries, timeBetweenTries);
                    }
                }
            }).ContinueWith(task => this.unitOfWork.Save());
        }

        /// <summary>
        /// Gets all jobs.
        /// </summary>
        /// <returns>
        /// List of jobs
        /// </returns>
        public virtual IEnumerable<Job> GetAllJobs()
        {
            return this.unitOfWork.JobRepository.Get();
        }

        /// <summary>
        /// Gets the job for a given form domain model.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>
        /// The job entity
        /// </returns>
        public virtual Job GetJob(IForm form)
        {
            var formValues = this.userDataRepository.GetFormValues(form.Id);

            return new Job
            {
                ItemId = form.ItemId,
                Data = JsonConvert.SerializeObject(formValues),
                Tasks = new Collection<Task>()
            };
        }

        /// <summary>
        /// Gets the task for a specific plug domain model.
        /// </summary>
        /// <param name="plug">The plug.</param>
        /// <returns>
        /// The task entity
        /// </returns>
        public virtual Task GetTask(ISavePlug plug)
        {
            return new Task
            {
                ItemId = plug.ItemId,
                LastTry = DateTime.Now
            };
        }

        /// <summary>
        /// Saves the specified job to the database.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>
        /// The job with new attributes (e.g. Id)
        /// </returns>
        public virtual Job Save(Job job)
        {
            this.unitOfWork.JobRepository.Insert(job);
            this.unitOfWork.Save();
            return job;
        }

        /// <summary>
        /// Resets the task retry count for a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>
        /// Boolean value if everything was ok.
        /// </returns>
        public virtual bool ResetTaskById(int taskId)
        {
            var task = this.unitOfWork.TaskRepository.Get(includeProperties: "Job").FirstOrDefault(t => t.Id == taskId);
            if (task == null) return false;

            task.NumberOfTries = 0;
            this.unitOfWork.Save();
            return true;
        }

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="language">The language.</param>
        /// <param name="job">The job.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        /// <param name="timeBetweenTries">The time between tries.</param>
        protected virtual void ExecuteJob(SiteContext site, Language language, Job job, int maxRetries, int timeBetweenTries)
        {
            try
            {
                this.logger.Debug(string.Format("Starting executing of job '{0}'", job.Id), this);
                
                var tasks = new List<System.Threading.Tasks.Task>();
                var form = this.contextService.LoadForm(job.ItemId.ToString(), true);
                var formValues = JsonConvert.DeserializeObject<IDictionary<string, object>>(job.Data);
                this.contextService.PopulateFormValues(form, formValues);

                foreach (var task in job.Tasks.Where(t => t.NumberOfTries <= maxRetries))
                {
                    this.logger.Debug(string.Format("Start executing task '{0}' of job '{1}'", task.Id, job.Id), this);
                    
                    // check if we already can execute the plug
                    if (task.NumberOfTries > 0 && !this.ShouldRun(task.LastTry, task.NumberOfTries, timeBetweenTries))
                    {
                        this.logger.Debug(string.Format("Task '{0}' from job '{1}' won't be executed due to exceeded max retries or time betweet retries", task.Id, job.Id), this);
                        continue;
                    }

                    // get the plug
                    var plug = form.SavePlugs.FirstOrDefault(p => p.ItemId == task.ItemId);
                    if (plug == null)
                    {
                        this.logger.Debug("Plug could not be loaded from form", this);
                        continue;
                    }

                    // execute the plug
                    tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() =>
                                {
                                    using (new SiteContextSwitcher(site))
                                    {
                                        using (new LanguageSwitcher(language))
                                        {
                                            this.ExecuteTask(job, task, form, plug, maxRetries);
                                        }
                                    }
                                }));
                }

                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
                if (!job.Tasks.Any())
                {
                    this.logger.Debug(string.Format("No more open tasks, delete job '{0}'", job.Id), this);
                    this.unitOfWork.JobRepository.Delete(job);
                }
            }
            catch (Exception exception)
            {
                this.logger.Error("Error while asynchronously execute job", this, exception);
            }
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="task">The task.</param>
        /// <param name="form">The form.</param>
        /// <param name="plug">The plug.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        protected virtual void ExecuteTask(Job job, Task task, IForm form, ISavePlug plug, int maxRetries)
        {
            try
            {
                this.logger.Debug(string.Format("Execute async plug '{0}' from task '{1}' in job '{2}' and form '{3}'", plug.ItemId, task.Id, job.Id, form.ItemId), this);
                plug.Execute(form);
                job.Tasks.Remove(task);
            }
            catch (Exception exception)
            {
                this.logger.Error("Error while asynchronously execute save plug", this, exception);
                task.NumberOfTries++;
                task.LastTry = DateTime.Now;

                if (task.NumberOfTries > maxRetries)
                {
                    this.logger.Debug(string.Format("Maximum number of retries for task '{0}' in job '{1}' exceeded, send failure email", task.Id, job.Id), this);
                    this.SendFailureEmail(job, task, exception);
                }
            }
        }

        /// <summary>
        /// Check if the task should run with the exponential backoff algorithm.
        /// </summary>
        /// <param name="lastTry">The last try.</param>
        /// <param name="numberOfTries">The number of tries.</param>
        /// <param name="timeBetweenTries">The time between tries.</param>
        /// <returns>Boolean value if the task with specific parameters should be run or not</returns>
        protected virtual bool ShouldRun(DateTime lastTry, int numberOfTries, int timeBetweenTries)
        {
            var backoffTime = Math.Pow(2, numberOfTries - 1) * timeBetweenTries;
            var nextTry = lastTry.AddMinutes(backoffTime);
            return DateTime.Now >= nextTry;
        }

        /// <summary>
        /// Sends the failure email.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="task">The task.</param>
        /// <param name="exception">The exception.</param>
        protected virtual void SendFailureEmail(Job job, Task task, Exception exception)
        {
            var from = Sitecore.Configuration.Settings.GetSetting("Flex.EmailAddresses.PlugExecutionFailureFrom");
            var to = Sitecore.Configuration.Settings.GetSetting("Flex.EmailAddresses.PlugExecutionFailureTo");
            var subject = this.dictionaryRepository.GetText("Plug Execution Failure Subject");
            var body = this.dictionaryRepository.GetText("Plug Execution Failure Body");

            var message = new MailMessage(from, to, subject, string.Format(body, job.ItemId, task.ItemId, exception.Message, exception.StackTrace));
            Sitecore.MainUtil.SendMail(message);
        }
    }
}
