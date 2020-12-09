namespace ContactService.Models
{
    /// <summary>
    /// The function configuration contract
    /// </summary>
    public interface IFunctionConfiguration
    {       
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>
        /// The name of the account.
        /// </value>
        string CosmosAccountName { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        string CosmosDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the name of the collection.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        string CosmosUserCollectionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the cosmos contact collection.
        /// </summary>
        /// <value>
        /// The name of the cosmos contact collection.
        /// </value>
        string CosmosContactCollectionName { get; set; }

        /// <summary>
        /// Gets or sets the database key.
        /// </summary>
        /// <value>
        /// The database key.
        /// </value>
        string CosmosDatabaseKey { get; set; }

        /// <summary>
        /// Gets or sets the cosmos database endpoint.
        /// </summary>
        /// <value>
        /// The cosmos database endpoint.
        /// </value>
        string CosmosDatabaseEndpoint { get; set; }
    }
}
