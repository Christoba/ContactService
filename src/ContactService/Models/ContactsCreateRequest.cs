namespace ContactService.Models
{
    /// <summary>
    /// The request to create contacts
    /// </summary>
    public class ContactsCreateRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsCreateRequest" /> class.
        /// </summary>
        public ContactsCreateRequest()
        {
            this.UserId = string.Empty;
            this.Count = 0;
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
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get;
            set;
        }
    }
}
