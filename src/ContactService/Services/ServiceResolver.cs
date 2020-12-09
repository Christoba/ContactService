namespace ContactService.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;

    /// <summary>
    /// The service resolver singleton.
    /// </summary>
    public sealed class ServiceResolver : IServiceResolver
    {      
        /// <summary>
        /// The HTTP Client
        /// </summary>
        public static readonly HttpClient HttpClientBacking = new HttpClient();

        /// <summary>
        /// The key vault Client
        /// </summary>
        public static readonly IKeyVaultClient KeyVaultClientBacking = new KeyVaultClient(
            new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));

        /// <summary>
        /// The default timeout in seconds
        /// </summary>
        private const int DefaultTimeoutInSeconds = 300;

        /// <summary>
        /// The lock
        /// </summary>
        private static readonly object Lock = new object();

        /// <summary>
        /// The document Client backing
        /// </summary>
        private static DocumentClient documentClientBacking = null;

        /// <summary>
        /// The instance
        /// </summary>
        private static IServiceResolver instance = null;

        /// <summary>
        /// The disposed backing.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Prevents a default instance of the <see cref="ServiceResolver"/> class from being created.
        /// </summary>
        private ServiceResolver()
        {         
        }     

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static IServiceResolver Instance
        {
            get
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        // Enable TLS 1.2
                        ServicePointManager.SecurityProtocol =
                            SecurityProtocolType.Tls12;

                        // Must be set only once for the Client before requests are made.
                        HttpClientBacking.Timeout = TimeSpan.FromSeconds(DefaultTimeoutInSeconds);

                        instance = new ServiceResolver();
                    }

                    return instance;
                }
            }
        }

        /// <inheritdoc />        
        public HttpClient HttpClient => HttpClientBacking;

        /// <inheritdoc />
        public IKeyVaultClient KeyVaultClient => KeyVaultClientBacking;

        /// <summary>
        /// Gets or sets the document Client.
        /// </summary>
        /// <value>
        /// The document Client.
        /// </value>
        public DocumentClient DocumentClient
        {
            get => documentClientBacking;

            set
            {
                if (documentClientBacking == null)
                {
                    documentClientBacking = value;
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            
            if (disposing)
            {
                KeyVaultClientBacking.Dispose();
                HttpClientBacking.Dispose();

                if (documentClientBacking != null)
                {
                    documentClientBacking.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
