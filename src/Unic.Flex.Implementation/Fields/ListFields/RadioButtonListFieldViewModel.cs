﻿namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    public class RadioButtonListFieldViewModel : ListFieldViewModel<string>
    {
        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/RadioButtonList";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_radiogroup");
        }
    }
}
