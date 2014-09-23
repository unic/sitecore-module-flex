﻿namespace Unic.Flex.Plugs
{
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using Sitecore.Sites;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Unic.Flex.Context;
    using Unic.Flex.Database;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
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
        /// Initializes a new instance of the <see cref="TaskService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="contextService">The context service.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="logger">The logger.</param>
        public TaskService(IUnitOfWork unitOfWork, IContextService contextService, IUserDataRepository userDataRepository, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.contextService = contextService;
            this.userDataRepository = userDataRepository;
            this.logger = logger;
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
            
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                using (new SiteContextSwitcher(site))
                {
                    try
                    {
                        var form = this.contextService.LoadForm(job.ItemId.ToString());
                        var formValues = JsonConvert.DeserializeObject<IDictionary<string, object>>(job.Data);
                        this.contextService.PopulateFormValues(form, formValues);

                        foreach (var plug in form.SavePlugs)
                        {
                            System.Threading.Tasks.Task.Factory.StartNew(
                                () =>
                                    {
                                        try
                                        {
                                            plug.Execute(form);

                                            //// todo: remove task from database because it's done
                                        }
                                        catch (Exception exception)
                                        {
                                            //// todo: increment retry count by 1
                                            this.logger.Error("Error while asynchronously execute save plug", this, exception);
                                        }
                                    });
                        }

                        //// todo: wait for all tasks and remove the "job" if all task have run
                    }
                    catch (Exception exception)
                    {
                        //// todo: handle exception correctly and do needed action (send email?)
                        this.logger.Error("Error while asynchronously execute job", this, exception);
                    }
                }
            });
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
    }
}
