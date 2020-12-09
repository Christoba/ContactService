namespace ContactService.Models
{
    /// <summary>
    /// The request to get a specific user
    /// </summary>
    public class UserGetRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserGetRequest" /> class.
        /// </summary>
        public UserGetRequest()
        {
            this.UserPartition = string.Empty;
            this.UserEmail = string.Empty;
        }

        /// <summary>
        /// Gets or sets the user partition.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserPartition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string UserEmail
        {
            get;
            set;
        }
    }
}
