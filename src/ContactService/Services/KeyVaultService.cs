namespace ContactService.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Repositories;

    /// <summary>
    /// The key vault service
    /// </summary>
    public class KeyVaultService : IKeyVaultService
    {
        /// <summary>
        /// The log service
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// The key vault Client
        /// </summary>
        private readonly IKeyVaultRepository keyVaultRepository;

        /// <summary>
        /// The key vault connection information
        /// </summary>
        private readonly IKeyVaultConnectionInfo keyVaultConnectionInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultService"/> class.
        /// </summary>
        /// <param name="keyVaultConnectionInfo">The key vault connection information.</param>
        /// <param name="logService">The log service.</param>
        /// <param name="keyVaultRepository">The key vault repository.</param>
        public KeyVaultService(IKeyVaultConnectionInfo keyVaultConnectionInfo, ILogService logService, IKeyVaultRepository keyVaultRepository)
        {
            this.keyVaultConnectionInfo = keyVaultConnectionInfo ?? throw new ArgumentNullException(nameof(keyVaultConnectionInfo));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));
            this.keyVaultRepository = keyVaultRepository ?? throw new ArgumentNullException(nameof(keyVaultRepository));
        }

        /// <inheritdoc />
        public async Task<KeyVaultSecretModel> GetKeyVaultSecretAsync(
            string secretName,
            CancellationToken token)
        {
            var bundle = await this.keyVaultRepository.GetSecretAsync(this.keyVaultConnectionInfo.ServerUrl, secretName, token)
                .ConfigureAwait(false);

            var secretModel = new KeyVaultSecretModel()
                                  {
                                      IdentifierWithVersion = bundle.SecretIdentifier.Identifier,
                                      Name = bundle.SecretIdentifier.Name,
                                      Value = bundle.Value
                                  };

            this.logService.LogInformation("Retrieved secret with name '{Name}' and Id '{Identifer}'", secretModel.Name, secretModel.IdentifierWithVersion);
            return secretModel;
        }
    }
}
