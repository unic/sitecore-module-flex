namespace Unic.Flex.Model.Steps
{
    using Unic.Flex.Model.Presentation;

    public abstract class StepBase : ItemBase, IPresentationComponent
    {
        public virtual bool IsActive { get; set; }

        public abstract string ViewName { get; }
    }
}
