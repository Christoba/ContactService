
namespace ContactService.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Newtonsoft.Json;
    using Serilog;
    using Serilog.Sinks.ILogger;
    using Services;
    using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;
    using ILogger = Microsoft.Extensions.Logging.ILogger;

    /// <summary>
    /// The function runner
    /// </summary>
    public class FunctionRunner
    {
        /// <summary>
        /// The service resolver
        /// </summary>
        private readonly IServiceResolver serviceResolver;

        /// <summary>
        /// The execution context
        /// </summary>
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// The log service
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionRunner" /> class.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        /// <param name="executionContext">The execution context.</param>
        /// <param name="logger">The logger.</param>
        public FunctionRunner(IServiceResolver serviceResolver, ExecutionContext executionContext, ILogger logger)
        {
            this.serviceResolver = serviceResolver ?? throw new ArgumentNullException(nameof(serviceResolver));
            this.executionContext = executionContext ?? throw new ArgumentNullException(nameof(executionContext));

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.logService = CreateSerilogLogService(logger);
        }

        /// <summary>
        /// Creates the users.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task CreateUsersAsync(int count, CancellationToken token)
        {
            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                await service.CreateUsersAsync(count, token).ConfigureAwait(false);

                this.logService.LogInformation(
                    "Successfully executed Create Users at {Timestamp}",
                    DateTime.UtcNow);
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Create Users. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Creates the contacts.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task CreateContactsAsync(ContactsCreateRequest request, CancellationToken token)
        {
            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                await service.CreateContactsAsync(request.UserId, request.Count, token).ConfigureAwait(false);

                this.logService.LogInformation(
                    "Successfully executed Create Contacts at {Timestamp}",
                    DateTime.UtcNow);
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Create Contacts. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The contacts.
        /// </returns>
        public IEnumerable<Resource> GetContacts(ContactsGetRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                var contacts = service.GetContacts(request);

                this.logService.LogInformation(
                    "Successfully executed Get at {Timestamp}",
                    DateTime.UtcNow);

                return contacts;
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Get. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The users.
        /// </returns>
        public IEnumerable<Resource> GetUsers(UsersGetRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                var users = service.GetUsers(request);

                this.logService.LogInformation(
                    "Successfully executed Get at {Timestamp}",
                    DateTime.UtcNow);

                return users;
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Get. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Deletes the users.
        /// </summary>
        public async Task DeleteUsersAsync(CancellationToken token)
        {
            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                await service.DeleteUsersAsync(token).ConfigureAwait(false);

                this.logService.LogInformation(
                    "Successfully executed Delete Users at {Timestamp}",
                    DateTime.UtcNow);
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Delete Users. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Deletes the contacts.
        /// </summary>
        public async Task DeleteContactsAsync(CancellationToken token)
        {
            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                await service.DeleteContactsAsync(token).ConfigureAwait(false);

                this.logService.LogInformation(
                    "Successfully executed Delete Contacts at {Timestamp}",
                    DateTime.UtcNow);
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Delete Contacts. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The user.
        /// </returns>
        public async Task<Resource> GetUser(UserGetRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                var user = await service.GetUserAsync(request).ConfigureAwait(false);

                this.logService.LogInformation(
                    "Successfully executed Get at {Timestamp}",
                    DateTime.UtcNow);

                return user;
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Get. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The contact.
        /// </returns>
        public async Task<Resource> GetContact(ContactGetRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var configurationBuilder = this.CreateConfigurationBuilder();

                var configReader = ServiceFactory.CreateConfigurationReader(configurationBuilder);
                var config = configReader.Read();

                var service = ServiceFactory.CreateContactService(
                    this.serviceResolver,
                    ConnectionHelper.CreateCosmosUserConnectionInfo(config),
                    ConnectionHelper.CreateCosmosContactConnectionInfo(config),
                    this.logService);

                var contact = await service.GetContactAsync(request).ConfigureAwait(false);

                this.logService.LogInformation(
                    "Successfully executed Get Contact at {Timestamp}",
                    DateTime.UtcNow);

                return contact;
            }
            catch (Exception e)
            {
                var errorMessage = $"Fatal exception during Get Contact. ErrorMessage message: {e.Message}";
                this.logService.LogError(e, errorMessage);
                throw;
            }
        }

        /// <summary>
        /// Creates the log service.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// The <see cref="ILogger" />.
        /// </returns>
        private static ILogService CreateSerilogLogService(ILogger logger)
        {
            var log = new LoggerConfiguration()
                .WriteTo.ILogger(logger)
                .CreateLogger();

            return new SerilogLogService(log);
        }

        /// <summary>
        /// Creates the configuration builder.
        /// </summary>
        /// <returns>
        /// The <see cref="IConfigurationRoot"/>.
        /// </returns>
        private IConfigurationRoot CreateConfigurationBuilder()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(this.executionContext.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return config;
        }
    }
}