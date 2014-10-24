namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using Unic.Flex.Analytics;
    using Unic.Flex.Context;
    using Unic.Flex.Model.DomainModel.Analytics;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Save plug to register a goal in the analytics
    /// </summary>
    [SitecoreType(TemplateId = "{9A7F5FA1-E267-4BF2-8D3C-4D487792B636}")]
    public class RegisterGoal : SavePlugBase
    {
        /// <summary>
        /// The analytics service
        /// </summary>
        private readonly IAnalyticsService analyticsService;

        /// <summary>
        /// The flex context
        /// </summary>
        private readonly IFlexContext flexContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterGoal" /> class.
        /// </summary>
        /// <param name="analyticsService">The analytics service.</param>
        /// <param name="flexContext">The flex context.</param>
        public RegisterGoal(IAnalyticsService analyticsService, IFlexContext flexContext)
        {
            this.analyticsService = analyticsService;
            this.flexContext = flexContext;
        }

        /// <summary>
        /// Gets a value indicating whether this plug should be executed asynchronous.
        /// </summary>
        /// <value>
        /// <c>true</c> if this plug should be executed asynchronous; otherwise, <c>false</c>.
        /// </value>
        public override bool IsAsync
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the goal.
        /// </summary>
        /// <value>
        /// The goal.
        /// </value>
        [SitecoreSharedField("Goal")]
        public virtual Goal Goal { get; set; }

        /// <summary>
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            if (this.Goal != null)
            {
                this.analyticsService.RegisterGoal(this.Goal, this.flexContext.Item);
            }
        }
    }
}
