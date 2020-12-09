namespace ContactService.Services
{
    using System;

    /// <summary>
    /// The log service contract.
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogDebug(string message, params object[] properties);

        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogDebug(Exception exception, string message, params object[] properties);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogInformation(string message, params object[] properties);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogInformation(Exception exception, string message, params object[] properties);

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogWarning(string message, params object[] properties);

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogWarning(Exception exception, string message, params object[] properties);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogError(string message, params object[] properties);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="properties">The properties.</param>
        void LogError(Exception exception, string message, params object[] properties);
    }
}
