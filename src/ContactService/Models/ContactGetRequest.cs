namespace ContactService.Models
{
    /// <summary>
    /// The request to get a specific contact
    /// </summary>
    public class ContactGetRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactGetRequest" /> class.
        /// </summary>
        public ContactGetRequest()
        {
            this.UserId = string.Empty;
            this.ContactId = string.Empty;
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
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public string ContactId
        {
            get;
            set;
        }
    }
}
