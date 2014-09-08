namespace Unic.Flex.Database
{
    using System.Data.Entity;
    using Unic.Flex.Model.Entities;

    public class FlexDbContext : DbContext
    {
        public FlexDbContext() : base("name=Flex")
        {
        }

        public virtual DbSet<Form> Forms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // add this because the sql server assembly won't copied if there is no strongly typed reference to it
            var addReference = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
