namespace ContactService.Services
{
    using System;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Repositories;

    /// <summary>
    /// The chef message service
    /// </summary>
    public static class ServiceFactory
    {
        /// <summary>
        /// Creates the configuration reader.
        /// </summary>
        /// <param name="configurationRoot">The configuration root.</param>
        /// <returns>
        /// The <see cref="IFunctionConfigurationReader" />.
        /// </returns>
        public static IFunctionConfigurationReader CreateConfigurationReader(IConfigurationRoot configurationRoot)
        {
            return new SettingsReader(configurationRoot);
        }

        /// <summary>
        /// Creates the cosmos failure repository.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        /// <param name="userConnectionInfo">The user connection information.</param>
        /// <param name="contactConnectionInfo">The contact connection information.</param>
        /// <returns>
        /// The <see cref="ICosmosConnectionInfo" />.
        /// </returns>
        public static ICosmosRepository CreateCosmosRepository(IServiceResolver serviceResolver, ICosmosConnectionInfo userConnectionInfo, ICosmosConnectionInfo contactConnectionInfo)
        {
            SetDocumentClient(serviceResolver, userConnectionInfo.EndpointUrl, userConnectionInfo.AuthKey); // same endpoint and auth key for both
            return new CosmosRepository(
                serviceResolver.DocumentClient,
                userConnectionInfo,
                contactConnectionInfo);
        }

        /// <summary>
        /// Creates the document service.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        /// <param name="userConnectionInfo">The user connection information.</param>
        /// <param name="contactConnectionInfo">The contact connection information.</param>
        /// <param name="logService">The log service.</param>
        /// <returns>
        /// The <see cref="IContactService" />.
        /// </returns>
        public static IContactService CreateContactService(
            IServiceResolver serviceResolver,
            ICosmosConnectionInfo userConnectionInfo,
            ICosmosConnectionInfo contactConnectionInfo,
            ILogService logService)
        {
            return new ContactService(
                CreateCosmosRepository(serviceResolver, userConnectionInfo, contactConnectionInfo),
                logService);
        }

        /// <summary>
        /// Sets the document Client.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="authKey">The authentication key.</param>
        private static void SetDocumentClient(IServiceResolver serviceResolver, Uri endpoint, string authKey)
        {
            if (serviceResolver.DocumentClient == null)
            {
                var failureClient = new DocumentClient(endpoint, authKey);
                serviceResolver.DocumentClient = failureClient;
            }
        }
    }
}
