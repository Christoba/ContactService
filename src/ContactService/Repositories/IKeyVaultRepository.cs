namespace ContactService.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault.Models;

    /// <summary>
    /// The key vault repository contract.
    /// </summary>
    public interface IKeyVaultRepository
    {
        /// <summary>
        /// Gets the secret.
        /// </summary>
        /// <param name="vaultBaseUrl">The vault base URL.</param>
        /// <param name="secretName">Name of the secret.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Thee <see cref="SecretBundle" />.
        /// </returns>
        Task<SecretBundle> GetSecretAsync(Uri vaultBaseUrl, string secretName, CancellationToken cancellationToken);
    }
}
