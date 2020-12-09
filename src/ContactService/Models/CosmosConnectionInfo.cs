namespace ContactService.Models
{
    using System;

    /// <summary>
    /// The cosmos connection info
    /// </summary>
    public class CosmosConnectionInfo : ICosmosConnectionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosConnectionInfo"/> class.
        /// </summary>
        public CosmosConnectionInfo()
        {
            this.DatabaseId = string.Empty;
            this.CollectionId = string.Empty;
            this.EndpointUrl = null;
            this.AuthKey = string.Empty;
        }

        /// <inheritdoc />
        public string DatabaseId { get; set; }

        /// <inheritdoc />
        public string CollectionId { get; set; }

        /// <inheritdoc />
        public Uri EndpointUrl { get; set; }

        /// <inheritdoc />
        public string AuthKey { get; set; }
    }
}
