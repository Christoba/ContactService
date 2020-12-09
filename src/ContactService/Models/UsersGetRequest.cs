namespace ContactService.Models
{
    /// <summary>
    /// The request to get users
    /// </summary>
    public class UsersGetRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersGetRequest" /> class.
        /// </summary>
        public UsersGetRequest()
        {
            this.UserPartition = string.Empty;
            this.Skip = 0;
            this.Take = 0;
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
        /// Gets or sets the skip.
        /// </summary>
        /// <value>
        /// The skip.
        /// </value>
        public int Skip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        /// <value>
        /// The take.
        /// </value>
        public int Take
        {
            get;
            set;
        }
    }
}
