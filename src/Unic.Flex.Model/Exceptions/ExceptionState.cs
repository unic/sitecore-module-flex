namespace Unic.Flex.Model.Exceptions
{
    using System.Collections.Generic;
    using System.Linq;

    public class ExceptionState : IExceptionState
    {
        public ExceptionState()
        {
            this.Messages = new List<string>();
        }
        
        public IList<string> Messages { get; private set; }

        public bool HasErrors
        {
            get
            {
                return this.Messages.Any();
            }
        }
    }
}
