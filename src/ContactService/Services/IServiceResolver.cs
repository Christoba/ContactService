namespace ContactService.Services
{
    using System;
    using System.Net.Http;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.KeyVault;

    /// <summary>
    /// The service resolver contract.
    /// </summary>
    public interface IServiceResolver : IDisposable
    {
        /// <summary>
        /// Gets the HTTP Client.
        /// </summary>
        /// <value>
        /// The HTTP Client.
        /// </value>
        HttpClient HttpClient { get; }        

        /// <summary>
        /// Gets the key vault Client.
        /// </summary>
        /// <value>
        /// The key vault Client.
        /// </value>
        IKeyVaultClient KeyVaultClient
        {
            get;
        }

        /// <summary>
        /// Gets or sets the failure document Client.
        /// </summary>
        /// <value>
        /// The document Client.
        /// </value>
        DocumentClient DocumentClient
        {
            get;
            set;
        }
    }
}
