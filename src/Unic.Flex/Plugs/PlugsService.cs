namespace Unic.Flex.Plugs
{
    using Sitecore.Diagnostics;
    using System;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.Exceptions;

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
        /// The exception state
        /// </summary>
        private readonly IExceptionState exceptionState;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlugsService"/> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="exceptionState">State of the exception.</param>
        public PlugsService(IUserDataRepository userDataRepository, ILogger logger, IExceptionState exceptionState)
        {
            this.userDataRepository = userDataRepository;
            this.logger = logger;
            this.exceptionState = exceptionState;
        }

        /// <summary>
        /// Executes the load plugs.
        /// </summary>
        /// <param name="form">The form.</param>
        public void ExecuteLoadPlugs(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

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
                    this.exceptionState.Messages.Add(plug.ErrorMessage);
                    this.logger.Error("Error while exeuting load plug", this, exception);
                    return;
                }
            }
        }

        /// <summary>
        /// Executes the save plugs.
        /// </summary>
        /// <param name="form">The form.</param>
        public void ExecuteSavePlugs(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

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
                    this.exceptionState.Messages.Add(plug.ErrorMessage);
                    this.logger.Error("Error while exeuting save plug", this, exception);
                    return;
                }
            }
        }
    }
}
