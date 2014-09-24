namespace Unic.Flex.Plugs
{
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using Sitecore.Sites;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Unic.Configuration;
    using Unic.Flex.Context;
    using Unic.Flex.Database;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;
    using Unic.Flex.Model.Entities;
    using Form = Unic.Flex.Model.DomainModel.Forms.Form;

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
        /// Initializes a new instance of the <see cref="TaskService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="contextService">The context service.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        public TaskService(IUnitOfWork unitOfWork, IContextService contextService, IUserDataRepository userDataRepository, ILogger logger, IConfigurationManager configurationManager)
        {
            this.unitOfWork = unitOfWork;
            this.contextService = contextService;
            this.userDataRepository = userDataRepository;
            this.logger = logger;
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// Executes all jobs.
        /// </summary>
        /// <param name="site">The site.</param>
        public virtual void ExecuteAll(SiteContext site)
        {
            var jobs = this.unitOfWork.JobRepository.Get();
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

            // get max retries
            var maxRetries = this.configurationManager.Get<GlobalConfiguration>(c => c.MaxRetries);

            // start thread and execute specific job
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                using (new SiteContextSwitcher(site))
                {
                    this.ExecuteJob(job, maxRetries);
                }
            }).ContinueWith(task => this.unitOfWork.Save());
        }

        /// <summary>
        /// Gets the job for a given form domain model.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>
        /// The job entity
        /// </returns>
        public virtual Job GetJob(Form form)
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
                ItemId = plug.ItemId
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
        /// Executes the job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        private void ExecuteJob(Job job, int maxRetries)
        {
            try
            {
                var tasks = new List<System.Threading.Tasks.Task>();
                var form = this.contextService.LoadForm(job.ItemId.ToString());
                var formValues = JsonConvert.DeserializeObject<IDictionary<string, object>>(job.Data);
                this.contextService.PopulateFormValues(form, formValues);

                foreach (var task in job.Tasks.Where(t => t.RetryCount < maxRetries))
                {
                    var plug = form.SavePlugs.FirstOrDefault(p => p.ItemId == task.ItemId);
                    if (plug == null) continue;

                    tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(() => this.ExecuteTask(job, task, form, plug)));
                }

                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
                if (!job.Tasks.Any())
                {
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
        private void ExecuteTask(Job job, Task task, Form form, ISavePlug plug)
        {
            try
            {
                plug.Execute(form);
                job.Tasks.Remove(task);
            }
            catch (Exception exception)
            {
                this.logger.Error("Error while asynchronously execute save plug", this, exception);
                task.RetryCount++;

                //// todo: send email if retry count is too high
            }
        }
    }
}
