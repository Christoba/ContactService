namespace ContactService.Models
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The key vault connection info
    /// </summary>
    public class KeyVaultConnectionInfo : IKeyVaultConnectionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultConnectionInfo"/> class.
        /// </summary>
        public KeyVaultConnectionInfo()
        {
            this.ServerUrl = null;
        }

        /// <inheritdoc />
        public Uri ServerUrl
        {
            get;
            set;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Server: '{0}'",
                this.ServerUrl);
        }
    }
}
