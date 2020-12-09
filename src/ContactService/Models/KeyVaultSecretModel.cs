namespace ContactService.Models
{
    using System.Globalization;

    /// <summary>
    /// The key vault secret model.
    /// </summary>
    public class KeyVaultSecretModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultSecretModel" /> class.
        /// </summary>
        public KeyVaultSecretModel()
        {
            this.IdentifierWithVersion = string.Empty;
            this.Name = string.Empty;
            this.Value = string.Empty;
        }

        /// <summary>
        /// Gets or sets the identifier with version.
        /// </summary>
        /// <value>
        /// The identifier with version.
        /// </value>
        public string IdentifierWithVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Name: '{0}', Identifier with Version: '{1}'",
                this.Name,
                this.IdentifierWithVersion);
        }
    }
}
