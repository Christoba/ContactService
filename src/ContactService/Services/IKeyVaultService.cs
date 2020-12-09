namespace ContactService.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    /// <summary>
    /// The key vault service contract.
    /// </summary>
    public interface IKeyVaultService
    {
        /// <summary>
        /// Gets the key vault secret.
        /// </summary>
        /// <param name="secretName">Name of the secret.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The <see cref="KeyVaultSecretModel"/>.
        /// </returns>
        Task<KeyVaultSecretModel> GetKeyVaultSecretAsync(string secretName, CancellationToken token);
    }
}
