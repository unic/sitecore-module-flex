namespace Unic.Flex.Core.Database
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Unic.Flex.Model.Entities;

    public class FlexDbContext : DbContext
    {
        public FlexDbContext() : base("name=Flex")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = Sitecore.Configuration.Settings.GetIntSetting("Flex.Database.CommandTimeout", 30); 
        }

        public virtual DbSet<Form> Forms { get; set; }

        public virtual DbSet<Session> Sessions { get; set; }

        public virtual DbSet<Job> Jobs { get; set; }

        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var addReference = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

            modelBuilder.Entity<Task>().HasKey(task => new { task.Id, task.JobId });
            modelBuilder.Entity<Task>().HasRequired(x => x.Job).WithMany(x => x.Tasks).HasForeignKey(x => x.JobId);
        }
    }
}
