﻿namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    public class CheckBoxListFieldViewModel : ListFieldViewModel<string[]>
    {
        // todo: client-side validator does not work

        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/CheckBoxList";
            }
        }
    }
}
