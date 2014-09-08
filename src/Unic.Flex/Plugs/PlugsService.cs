namespace Unic.Flex.Plugs
{
    using Sitecore.Diagnostics;
    using System;
    using Unic.Flex.Context;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;

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
        /// Initializes a new instance of the <see cref="PlugsService" /> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="logger">The logger.</param>
        public PlugsService(IUserDataRepository userDataRepository, ILogger logger)
        {
            this.userDataRepository = userDataRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Executes the load plugs.
        /// </summary>
        /// <param name="context">The context.</param>
        public void ExecuteLoadPlugs(IFlexContext context)
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
                    this.logger.Error("Error while exeuting load plug", this, exception);
                    return;
                }
            }
        }

        /// <summary>
        /// Executes the save plugs.
        /// </summary>
        /// <param name="context">The context.</param>
        public void ExecuteSavePlugs(IFlexContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            // get the form
            var form = context.Form;
            if (form == null) return;

            // todo: we currently don't know exactly how we should execute these plugs (i.e. sync/async, revirsible, etc.) -> so change the implementation when it's clear
            foreach (var plug in form.SavePlugs)
            {
                try
                {
                    plug.Execute(form);
                }
                catch (Exception exception)
                {
                    // todo: Include rollback
                    context.ErrorMessage = form.ErrorMessage;
                    this.logger.Error("Error while exeuting save plug", this, exception);
                    return;
                }
            }
        }
    }
}
