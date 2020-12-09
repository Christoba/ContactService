namespace ContactService.Models
{
    /// <summary>
    /// The function configuration contract
    /// </summary>
    public class FunctionConfiguration : IFunctionConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionConfiguration"/> class.
        /// </summary>
        public FunctionConfiguration()
        {           
            this.CosmosAccountName = string.Empty;
            this.CosmosUserCollectionName = string.Empty;
            this.CosmosContactCollectionName = string.Empty;
            this.CosmosDatabaseKey = string.Empty;
            this.CosmosDatabaseName = string.Empty;
            this.CosmosDatabaseEndpoint = string.Empty;
        }
       
        /// <inheritdoc />
        public string CosmosAccountName { get; set; }

        /// <inheritdoc />
        public string CosmosDatabaseName { get; set; }

        /// <inheritdoc />
        public string CosmosUserCollectionName { get; set; }

        /// <inheritdoc />
        public string CosmosContactCollectionName { get; set; }

        /// <inheritdoc />
        public string CosmosDatabaseKey { get; set; }

        /// <inheritdoc />
        public string CosmosDatabaseEndpoint { get; set; }
    }
}
