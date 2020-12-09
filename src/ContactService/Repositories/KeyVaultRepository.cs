namespace ContactService.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;

    /// <summary>
    /// The key vault repository
    /// </summary>
    /// <remarks>
    /// This repository wraps the <see cref="IKeyVaultClient"/>, which uses extension methods.
    /// </remarks>
    public class KeyVaultRepository : IKeyVaultRepository
    {
        /// <summary>
        /// The key vault Client
        /// </summary>
        private readonly IKeyVaultClient keyVaultClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultRepository"/> class.
        /// </summary>
        /// <param name="keyVaultClient">The key vault Client.</param>
        public KeyVaultRepository(IKeyVaultClient keyVaultClient)
        {
            this.keyVaultClient = keyVaultClient ?? throw new ArgumentNullException(nameof(keyVaultClient));
        }

        /// <inheritdoc />
        public Task<SecretBundle> GetSecretAsync(Uri vaultBaseUrl, string secretName, CancellationToken cancellationToken)
        {
            if (vaultBaseUrl == null)
            {
                throw new ArgumentNullException(nameof(vaultBaseUrl));
            }

            return this.keyVaultClient.GetSecretAsync(vaultBaseUrl.ToString(), secretName, cancellationToken);
        }
    }
}
