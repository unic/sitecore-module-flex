namespace Unic.Flex.Core.Plugs
{
    using System.Collections.Generic;
    using Sitecore.Sites;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;
    using Unic.Flex.Model.Entities;

    /// <summary>
    /// Interface for the plug task system
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Executes all jobs.
        /// </summary>
        /// <param name="site">The site.</param>
        void ExecuteAll(SiteContext site);
        
        /// <summary>
        /// Executes the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="site">The site.</param>
        void Execute(Job job, SiteContext site);

        /// <summary>
        /// Gets all jobs.
        /// </summary>
        /// <returns>List of jobs</returns>
        IEnumerable<Job> GetAllJobs();

        /// <summary>
        /// Gets the job for a given form domain model.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>The job entity</returns>
        Job GetJob(Model.DomainModel.Forms.Form form);

        /// <summary>
        /// Gets the task for a specific plug domain model.
        /// </summary>
        /// <param name="plug">The plug.</param>
        /// <returns>The task entity</returns>
        Task GetTask(ISavePlug plug);

        /// <summary>
        /// Saves the specified job to the database.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>The job with new attributes (e.g. Id)</returns>
        Job Save(Job job);

        /// <summary>
        /// Resets the task retry count for a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>
        /// Boolean value if everything was ok.
        /// </returns>
        bool ResetTaskById(int taskId);
    }
}
