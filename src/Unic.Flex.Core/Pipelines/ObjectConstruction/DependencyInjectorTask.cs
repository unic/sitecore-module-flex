namespace Unic.Flex.Core.Pipelines.ObjectConstruction
{
    using System.Dynamic;
    using Glass.Mapper.Pipelines.ObjectConstruction;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Glass Mapper task to create an object and inject classes using registered IoC container
    /// </summary>
    public class DependencyInjectorTask : AbstractObjectConstructionTask
    {
        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Execute(ObjectConstructionArgs args)
        {
            // check that no other task has created an object and that this is a dynamic object
            if (args.Result != null || args.Configuration.Type.IsAssignableFrom(typeof(IDynamicMetaObjectProvider)))
            {
                base.Execute(args);
                return;
            }
            
            // create instance using IoC container
            var obj = DependencyResolver.Resolve(args.Configuration.Type);

            // map properties from item to model
            args.Configuration.MapPropertiesToObject(obj, args.Service, args.AbstractTypeCreationContext);

            // set the new object as the returned result
            args.Result = obj;

            base.Execute(args);
        }
    }
}
