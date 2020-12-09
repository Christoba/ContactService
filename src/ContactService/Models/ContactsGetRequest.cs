namespace ContactService.Models
{
    /// <summary>
    /// The request to get contacts
    /// </summary>
    public class ContactsGetRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsGetRequest" /> class.
        /// </summary>
        public ContactsGetRequest()
        {
            this.UserId = string.Empty;
            this.Skip = 0;
            this.Take = 0;
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId
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
