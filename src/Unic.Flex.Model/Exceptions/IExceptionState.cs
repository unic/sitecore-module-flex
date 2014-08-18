namespace Unic.Flex.Model.Exceptions
{
    using System.Collections.Generic;

    public interface IExceptionState
    {
        IList<string> Messages { get; }

        bool HasErrors { get; }
    }
}
