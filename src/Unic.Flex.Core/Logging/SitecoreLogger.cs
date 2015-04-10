namespace Unic.Flex.Core.Logging
{
    using System;
    using Sitecore.Diagnostics;

    /// <summary>
    /// Logger implementation for logging to Sitecore log file.
    /// </summary>
    public class SitecoreLogger : ILogger
    {
        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner.</param>
        public virtual void Debug(string message, object owner)
        {
            Log.Debug(this.FormatMessage(message), owner);
        }
        
        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner/sender of the message.</param>
        public virtual void Info(string message, object owner)
        {
            Log.Info(this.FormatMessage(message), owner);
        }

        /// <summary>
        /// Logs a warn message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner/sender of the message.</param>
        public virtual void Warn(string message, object owner)
        {
            Log.Warn(this.FormatMessage(message), owner);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="owner">The owner/sender of the message.</param>
        /// <param name="exception">The exception, can also be null.</param>
        public virtual void Error(string message, object owner, Exception exception = null)
        {
            Log.Error(this.FormatMessage(message), exception, owner);
        }

        /// <summary>
        /// Formats the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Message with Flex prefix.</returns>
        private string FormatMessage(string message)
        {
            return string.Format("FLEX :: {0} :: Url {1}", message, Sitecore.Web.WebUtil.GetFullUrl(Sitecore.Web.WebUtil.GetRawUrl()));
        }
    }
}
