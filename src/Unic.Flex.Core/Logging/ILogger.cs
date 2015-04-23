namespace Unic.Flex.Core.Logging
{
    using System;

    /// <summary>
    /// Interface used for logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        void Debug(string message, object owner);
        
        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner/sender of the message.</param>
        void Info(string message, object owner);

        /// <summary>
        /// Logs a warn message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner/sender of the message.</param>
        /// <param name="exception">The exception.</param>
        void Warn(string message, object owner, Exception exception = null);

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner/sender of the message.</param>
        /// <param name="exception">The exception, can also be null.</param>
        void Error(string message, object owner, Exception exception = null);
    }
}
