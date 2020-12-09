namespace ContactService.Models
{
    using System;

    /// <summary>
    /// The cosmos connection info contract
    /// </summary>
    public interface ICosmosConnectionInfo
    {
        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        string DatabaseId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection identifier.
        /// </summary>
        /// <value>
        /// The collection identifier.
        /// </value>
        string CollectionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the endpoint URL.
        /// </summary>
        /// <value>
        /// The endpoint URL.
        /// </value>
        Uri EndpointUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authentication key.
        /// </summary>
        /// <value>
        /// The authentication key.
        /// </value>
        string AuthKey
        {
            get;
            set;
        }
    }
}
