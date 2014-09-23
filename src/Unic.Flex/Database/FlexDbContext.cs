namespace Unic.Flex.Database
{
    using System.Data.Entity;
    using Unic.Flex.Model.Entities;

    /// <summary>
    /// Database context
    /// </summary>
    public class FlexDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlexDbContext"/> class.
        /// </summary>
        public FlexDbContext() : base("name=Flex")
        {
        }

        /// <summary>
        /// Gets or sets the forms.
        /// </summary>
        /// <value>
        /// The forms.
        /// </value>
        public virtual DbSet<Form> Forms { get; set; }

        /// <summary>
        /// Gets or sets the jobs.
        /// </summary>
        /// <value>
        /// The jobs.
        /// </value>
        public virtual DbSet<Job> Jobs { get; set; }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // add this because the sql server assembly won't copied if there is no strongly typed reference to it
            var addReference = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

            // add needed relationships
            modelBuilder.Entity<Task>().HasKey(task => new { task.Id, task.JobId });
            modelBuilder.Entity<Task>().HasRequired(x => x.Job).WithMany(x => x.Tasks).HasForeignKey(x => x.JobId);
        }
    }
}
