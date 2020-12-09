namespace ContactService.Services
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The settings helper
    /// </summary>
    public class SettingHelper
    {
        /// <summary>
        /// The cosmos database account name setting
        /// </summary>
        private const string CosmosDbAccountNameSetting = "CosmosDbAccountName";

        /// <summary>
        /// The cosmos database database name setting
        /// </summary>
        private const string CosmosDbDatabaseNameSetting = "CosmosDbDatabaseName";


        /// <summary>
        /// The cosmos database collection name user setting
        /// </summary>
        private const string CosmosDbCollectionNameUserSetting = "CosmosDbCollectionNameUser";

        /// <summary>
        /// The cosmos database collection name contact setting
        /// </summary>
        private const string CosmosDbCollectionNameContactSetting = "CosmosDbCollectionNameContact";

        /// <summary>
        /// The cosmos database endpoint setting
        /// </summary>
        private const string CosmosDbEndpointSetting = "CosmosDbEndpoint";

        /// <summary>
        /// The cosmos authentication key setting
        /// </summary>
        private const string CosmosAuthKeySetting = "CosmosAuthKey";

        /// <summary>
        /// The configuration root
        /// </summary>
        private readonly IConfigurationRoot configurationRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingHelper"/> class.
        /// </summary>
        /// <param name="configurationRoot">The configuration root.</param>
        public SettingHelper(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
        }

        /// <summary>
        /// Gets the name of the cosmos database account.
        /// </summary>
        /// <returns>
        /// The setting.
        /// </returns>
        public string GetCosmosDbAccountName()
        {
            return this.GetAppSetting<string>(CosmosDbAccountNameSetting);
        }

        /// <summary>
        /// Gets the name of the cosmos database database.
        /// </summary>
        /// <returns>
        /// The setting.
        /// </returns>
        public string GetCosmosDbDatabaseName()
        {
            return this.GetAppSetting<string>(CosmosDbDatabaseNameSetting);
        }

        /// <summary>
        /// Gets the cosmos database collection name user.
        /// </summary>
        /// <returns>
        /// The setting.
        /// </returns>
        public string GetCosmosDbCollectionNameUser()
        {
            return this.GetAppSetting<string>(CosmosDbCollectionNameUserSetting);
        }

        /// <summary>
        /// Gets the cosmos database collection name contact.
        /// </summary>
        /// <returns>
        /// The setting.
        /// </returns>
        public string GetCosmosDbCollectionNameContact()
        {
            return this.GetAppSetting<string>(CosmosDbCollectionNameContactSetting);
        }

        /// <summary>
        /// Gets the cosmos database endpoint.
        /// </summary>
        /// <returns></returns>
        public string GetCosmosDbEndpoint()
        {
            return this.GetAppSetting<string>(CosmosDbEndpointSetting);
        }

        /// <summary>
        /// Gets the cosmos authentication key.
        /// </summary>
        /// <returns>
        /// The setting.
        /// </returns>
        public string GetCosmosAuthKey()
        {
            return this.GetAppSetting<string>(CosmosAuthKeySetting);
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <typeparam name="T">The setting type.</typeparam>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>
        /// The application setting.
        /// </returns>
        /// <exception cref="InvalidOperationException">If the setting does not exist or is empty.</exception>
        private T GetAppSetting<T>(string settingName)
        {
            var value = this.configurationRoot[settingName];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Could not read Application Setting '{settingName}'");
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
    }
}
