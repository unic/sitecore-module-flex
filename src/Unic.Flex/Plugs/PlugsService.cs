namespace Unic.Flex.Plugs
{
    using System;
    using System.Linq;
    using Sitecore.Diagnostics;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;
    using Unic.Flex.Model.Exceptions;

    public class PlugsService : IPlugsService
    {
        private readonly IUserDataRepository userDataRepository;

        private readonly ILogger logger;

        private readonly IExceptionState exceptionState;

        public PlugsService(IUserDataRepository userDataRepository, ILogger logger, IExceptionState exceptionState)
        {
            this.userDataRepository = userDataRepository;
            this.logger = logger;
            this.exceptionState = exceptionState;
        }
        
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
