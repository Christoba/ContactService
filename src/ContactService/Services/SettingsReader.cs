namespace ContactService.Services
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Models;

    /// <summary>
    /// The settings configuration reader.
    /// </summary>
    public class SettingsReader : IFunctionConfigurationReader
    {
        /// <summary>
        /// The configuration root
        /// </summary>
        private readonly IConfigurationRoot configurationRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsReader"/> class.
        /// </summary>
        /// <param name="configurationRoot">The configuration root.</param>
        public SettingsReader(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
        }

        /// <inheritdoc />
        public IFunctionConfiguration Read()
        {
            var settingHelper = new SettingHelper(this.configurationRoot);

            var accountName = settingHelper.GetCosmosDbAccountName();
            var databaseName = settingHelper.GetCosmosDbDatabaseName();
            var userCollectionName = settingHelper.GetCosmosDbCollectionNameUser();
            var contactCollectionName = settingHelper.GetCosmosDbCollectionNameContact();
            var databaseKey = settingHelper.GetCosmosAuthKey();
            var endpoint = settingHelper.GetCosmosDbEndpoint();

            return new FunctionConfiguration()
                       {
                           CosmosAccountName = accountName,
                           CosmosUserCollectionName = userCollectionName,
                           CosmosContactCollectionName = contactCollectionName,
                           CosmosDatabaseName = databaseName,
                           CosmosDatabaseKey = databaseKey,
                           CosmosDatabaseEndpoint = endpoint
                       };
        }
    }
}
