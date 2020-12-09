namespace ContactService.Services
{
    using System;
    using Models;

    /// <summary>
    /// The connection helper
    /// </summary>
    public static class ConnectionHelper
    {
        /// <summary>
        /// Creates the cosmos user connection information.
        /// </summary>
        /// <param name="dbConfiguration">The DB configuration.</param>
        /// <returns>
        /// The <see cref="ICosmosConnectionInfo"/>.
        /// </returns>
        public static ICosmosConnectionInfo CreateCosmosUserConnectionInfo(IFunctionConfiguration dbConfiguration)
        {
            return new CosmosConnectionInfo()
            {
                CollectionId = dbConfiguration.CosmosUserCollectionName,
                DatabaseId = dbConfiguration.CosmosDatabaseName,
                AuthKey = dbConfiguration.CosmosDatabaseKey,
                EndpointUrl = new Uri(
                               $"https://{dbConfiguration.CosmosAccountName}.{dbConfiguration.CosmosDatabaseEndpoint}:443/")
            };
        }

        /// <summary>
        /// Creates the cosmos contact connection information.
        /// </summary>
        /// <param name="dbConfiguration">The database configuration.</param>
        /// <returns>
        /// The <see cref="ICosmosConnectionInfo"/>.
        /// </returns>
        public static ICosmosConnectionInfo CreateCosmosContactConnectionInfo(IFunctionConfiguration dbConfiguration)
        {
            return new CosmosConnectionInfo()
            {
                CollectionId = dbConfiguration.CosmosContactCollectionName,
                DatabaseId = dbConfiguration.CosmosDatabaseName,
                AuthKey = dbConfiguration.CosmosDatabaseKey,
                EndpointUrl = new Uri(
                    $"https://{dbConfiguration.CosmosAccountName}.{dbConfiguration.CosmosDatabaseEndpoint}:443/")
            };
        }
    }
}
