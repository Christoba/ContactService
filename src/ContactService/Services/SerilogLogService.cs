namespace ContactService.Services
{
    using System;
    using Serilog;

    /// <summary>
    /// The log service
    /// </summary>
    public class SerilogLogService : ILogService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLogService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SerilogLogService(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void LogDebug(string message, params object[] properties)
        {
            this.logger.Debug(message, properties);
        }

        /// <inheritdoc />
        public void LogDebug(Exception exception, string message, params object[] properties)
        {
            this.logger.Debug(exception, message, properties);
        }

        /// <inheritdoc />
        public void LogInformation(string message, params object[] properties)
        {
            this.logger.Information(message, properties);
        }

        /// <inheritdoc />
        public void LogInformation(Exception exception, string message, params object[] properties)
        {
            this.logger.Information(exception, message, properties);
        }

        /// <inheritdoc />
        public void LogWarning(string message, params object[] properties)
        {
            this.logger.Warning(message, properties);
        }

        /// <inheritdoc />
        public void LogWarning(Exception exception, string message, params object[] properties)
        {
            this.logger.Warning(exception, message, properties);
        }

        /// <inheritdoc />
        public void LogError(string message, params object[] properties)
        {
            this.logger.Error(message, properties);
        }

        /// <inheritdoc />
        public void LogError(Exception exception, string message, params object[] properties)
        {
            this.logger.Error(exception, message, properties);
        }
    }
}
