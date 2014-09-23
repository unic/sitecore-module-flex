namespace Unic.Flex.Plugs
{
    using Sitecore.Diagnostics;
    using System;
    using System.Linq;
    using Unic.Configuration;
    using Unic.Flex.Context;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
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
            if (this.userDataRepository.IsFormStored(form.Id)) return;

            foreach (var plug in form.LoadPlugs)
            {
                try
                {
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
            //// todo: change the checkbox in the config to be a droplink instead of checkbox and get this value here from global config -> can be done after checkbox upgrade
            var isAsyncExecutionAllowed = true; // this.configurationManager.Get<GlobalConfiguration>(c => c.IsAsyncExecutionAllowed);
            var hasAsyncPlug = isAsyncExecutionAllowed && form.SavePlugs.Any(plug => plug.IsAsync);

            try
            {
                // add the form to the database
                Job job = null;
                if (hasAsyncPlug)
                {
                    job = this.taskService.GetJob(form);
                }

                // execute the plugs
                foreach (var plug in form.SavePlugs)
                {
                    if (isAsyncExecutionAllowed && plug.IsAsync)
                    {
                        job.Tasks.Add(this.taskService.GetTask(plug));
                    }
                    else
                    {
                        plug.Execute(form);
                    }
                }

                // save the data to database and start the task to execute the async tasks
                if (hasAsyncPlug)
                {
                    this.taskService.Save(job);
                    this.taskService.Execute(job, Sitecore.Context.Site); //// todo: do not access the sitecore context here directly
                }
            }
            catch (Exception exception)
            {
                // todo: Include rollback (delete async tasks and rollback sync plugs)
                context.ErrorMessage = form.ErrorMessage;
                this.logger.Error("Error while executing save plug", this, exception);
            }
        }
    }
}
