namespace Unic.Flex.Core.Plugs
{
    using System;
    using System.Linq;
    using System.Web;
    using Sitecore.Diagnostics;
    using Configuration.Core;
    using Context;
    using Logging;
    using Model.Configuration;
    using Model.Entities;
    using Model.Forms;
    using Model.Plugs;
    using Rules;

    public class PlugsService : IPlugsService
    {
        private readonly ILogger logger;
        private readonly IConfigurationManager configurationManager;
        private readonly ITaskService taskService;

        public PlugsService(ILogger logger, IConfigurationManager configurationManager, ITaskService taskService)
        {
            this.logger = logger;
            this.configurationManager = configurationManager;
            this.taskService = taskService;
        }

        public virtual void ExecuteLoadPlugs(IFlexContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            var form = context.Form;
            if (form == null) return;

            // check if we need to execute the load plugs -> only the first time
            var executeLoadPlugsOnNonHttpGet = Sitecore.Configuration.Settings.GetBoolSetting(Definitions.Constants.AllowLoadPlugsOnNonHttpGet, false);
            var isHttpGetRequest = HttpContext.Current.Request.HttpMethod == "GET";

            if (executeLoadPlugsOnNonHttpGet)
            {
                if (HttpContext.Current == null || form.ActiveStep.StepNumber != 1) return;
            }
            else
            {
                if (HttpContext.Current == null || !isHttpGetRequest || form.ActiveStep.StepNumber != 1) return;
            }

            foreach (var plug in form.LoadPlugs)
            {
                if (executeLoadPlugsOnNonHttpGet && !isHttpGetRequest && !plug.IgnoreHttpMethodExecutionFilter) continue;

                try
                {
                    this.logger.Debug($"Execute load plug '{plug.ItemId}' for form '{form.ItemId}'", this);
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
                    this.logger.Debug($"Form '{form.ItemId}' has async plugs", this);
                    job = this.taskService.GetJob(form);
                }

                // execute the plugs
                foreach (var plug in form.SavePlugs)
                {
                    if (isAsyncExecutionAllowed && plug.IsAsync)
                    {
                        this.logger.Debug($"Add async save plug '{plug.ItemId}' as background job for form '{form.ItemId}'", this);
                        job.Tasks.Add(this.taskService.GetTask(plug));
                    }
                    else
                    {
                        if (this.CanExecute(form, plug))
                        {
                            this.logger.Debug($"Execute sync save plug '{plug.ItemId}' for form '{form.ItemId}'", this);
                            plug.Execute(form);
                        }
                    }
                }

                // save the data to database and start the task to execute the async tasks
                if (hasAsyncPlug)
                {
                    job = this.taskService.Save(job);
                    this.logger.Debug($"Saved job '{job.Id}' for form '{form.ItemId}' to database", this);

                    this.taskService.Execute(job, context.SiteContext);
                }
            }
            catch (Exception exception)
            {
                context.ErrorMessage = form.ErrorMessage;
                this.logger.Error("Error while executing save plug", this, exception);
            }
        }

        /// <summary>
        /// Check if the save plug can be executed.
        /// </summary>
        /// <c>true</c> if all conditions are met; otherwise, <c>false</c>.
        /// <param name="form"></param>
        /// <param name="plug"></param>
        private bool CanExecute(IForm form, ISavePlug plug)
        {
            var ruleContext = new FlexFormRuleContext();
            ruleContext.Form = form;

            return !plug.ConditionalRule.Rules.Any() || plug.ConditionalRule.Rules.All(rule => rule.Evaluate(ruleContext));
        }
    }
}
