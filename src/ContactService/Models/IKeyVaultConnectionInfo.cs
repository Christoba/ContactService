namespace ContactService.Models
{
    using System;

    /// <summary>
    /// The key vault connection info contract
    /// </summary>
    public interface IKeyVaultConnectionInfo
    {
        /// <summary>
        /// Gets or sets the name of the server URL.
        /// </summary>
        /// <value>
        /// The server URL.
        /// </value>
        Uri ServerUrl
        {
            get;
            set;
        }
    }
}