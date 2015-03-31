namespace Unic.Flex.Core.Plugs
{
    using System;
    using System.Linq;
    using System.Web;
    using Sitecore.Diagnostics;
    using Unic.Configuration;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Entities;

    /// <summary>
    /// Service for the plug framework.
    /// </summary>
    public class PlugsService : IPlugsService
    {
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
        /// The task service
        /// </summary>
        private readonly ITaskService taskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlugsService" /> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="taskService">The task service.</param>
        public PlugsService(IUserDataRepository userDataRepository, ILogger logger, IConfigurationManager configurationManager, ITaskService taskService)
        {
            this.userDataRepository = userDataRepository;
            this.logger = logger;
            this.configurationManager = configurationManager;
            this.taskService = taskService;
        }

        /// <summary>
        /// Executes the load plugs.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void ExecuteLoadPlugs(IFlexContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            // get the form
            var form = context.Form;
            if (form == null) return;
            
            // check if we need to execute the load plugs -> only the first time
            if (HttpContext.Current == null || HttpContext.Current.Request.HttpMethod != "GET"
                || form.ActiveStep.StepNumber != 1) return;

            foreach (var plug in form.LoadPlugs)
            {
                try
                {
                    this.logger.Debug(string.Format("Execute load plug '{0}' for form '{1}'", plug.ItemId, form.ItemId), this);
                    plug.Execute(form);
                }
                catch (Exception exception)
                {
                    context.ErrorMessage = form.ErrorMessage;
                    this.logger.Error("Error while executing load plug", this, exception);
                    return;
                }
            }
        }

        /// <summary>
        /// Executes the save plugs.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void ExecuteSavePlugs(IFlexContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            // get the form
            var form = context.Form;
            if (form == null) return;

            // does the form has at least one async plug?
            var isAsyncExecutionAllowed = this.configurationManager.Get<GlobalConfiguration>(c => c.IsAsyncExecutionAllowed);
            var hasAsyncPlug = isAsyncExecutionAllowed && form.SavePlugs.Any(plug => plug.IsAsync);

            try
            {
                // add the form to the database
                Job job = null;
                if (hasAsyncPlug)
                {
                    this.logger.Debug(string.Format("Form '{0}' has async plugs", form.ItemId), this);
                    job = this.taskService.GetJob(form);
                }

                // execute the plugs
                foreach (var plug in form.SavePlugs)
                {
                    if (isAsyncExecutionAllowed && plug.IsAsync)
                    {
                        this.logger.Debug(string.Format("Add async save plug '{0}' as background job for form '{1}'", plug.ItemId, form.ItemId), this);
                        job.Tasks.Add(this.taskService.GetTask(plug));
                    }
                    else
                    {
                        this.logger.Debug(string.Format("Execute sync save plug '{0}' for form '{1}'", plug.ItemId, form.ItemId), this);
                        plug.Execute(form);
                    }
                }

                // save the data to database and start the task to execute the async tasks
                if (hasAsyncPlug)
                {
                    job = this.taskService.Save(job);
                    this.logger.Debug(string.Format("Saved job '{0}' for form '{1}' to database", job.Id, form.ItemId), this);

                    this.taskService.Execute(job, context.SiteContext);
                }
            }
            catch (Exception exception)
            {
                context.ErrorMessage = form.ErrorMessage;
                this.logger.Error("Error while executing save plug", this, exception);
            }
        }
    }
}
