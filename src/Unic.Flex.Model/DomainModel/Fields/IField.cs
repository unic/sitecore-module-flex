namespace Unic.Flex.Model.DomainModel.Fields
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Validation;

    public interface IField<TValue> : IField
    {
        new TValue Value { get; set; }
    }

    public interface IField
    {
        object Value { get; set; }

        string ViewName { get; }

        string Key { get; }
        
        string Label { get; set; }

        bool IsRequired { get; set; }

        string ValidationMessage { get; set; }

        IEnumerable<IValidator> Validators { get; set; }
    }
}
