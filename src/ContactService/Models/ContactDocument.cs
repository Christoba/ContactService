namespace ContactService.Models
{
    using Microsoft.Azure.Documents;

    /// <summary>
    /// The contact document.
    /// </summary>
    public class ContactDocument : Resource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDocument"/> class.
        /// </summary>
        public ContactDocument()
        {
            this.ContactId = string.Empty;
            this.UserId = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.ContactEmail = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDocument"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        public ContactDocument(Document document)
            : base(document)
        {
            this.Initialize(document);
        }

        /// <summary>
        /// Gets or sets the user partition.
        /// </summary>
        /// <value>
        /// The user partition.
        /// </value>
        public string ContactId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Initializes the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        private void Initialize(Document document)
        {
            this.ContactId = document.GetPropertyValue<string>("ContactId");
            this.UserId = document.GetPropertyValue<string>("UserId");
            this.FirstName = document.GetPropertyValue<string>("FirstName");
            this.LastName = document.GetPropertyValue<string>("LastName");
            this.ContactEmail = document.GetPropertyValue<string>("ContactEmail");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"ContactId:{this.ContactId} UserId:{this.UserId} Email:{this.ContactEmail}";
        }
    }
}
