namespace Unic.Flex.Model.Fields.InputFields
{
    using Unic.Flex.Model.Validators;

    /// <summary>
    /// Honepot spam protection field
    /// </summary>
    public class HoneypotField : InputField<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HoneypotField"/> class.
        /// </summary>
        public HoneypotField()
        {
            this.DefaultValidators.Add(new HoneypotValidator());
        }
        
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/Honeypot";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
            this.AddCssClass("flex_singletextfield info3-block");
        }
    }
}
