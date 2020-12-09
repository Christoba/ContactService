namespace ContactService.Models
{
    using Microsoft.Azure.Documents;

    /// <summary>
    /// The user document.
    /// </summary>
    public class UserDocument : Resource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDocument"/> class.
        /// </summary>
        public UserDocument()
        {
            this.UserPartition = string.Empty;
            this.UserId = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.UserEmail = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDocument"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        public UserDocument(Document document)
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
        public string UserPartition { get; set; }

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
        public string UserEmail { get; set; }

        /// <summary>
        /// Initializes the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        private void Initialize(Document document)
        {
            this.UserPartition = document.GetPropertyValue<string>("UserPartition");
            this.UserId = document.GetPropertyValue<string>("UserId");
            this.FirstName = document.GetPropertyValue<string>("FirstName");
            this.LastName = document.GetPropertyValue<string>("LastName");
            this.UserEmail = document.GetPropertyValue<string>("UserEmail");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"UserId:{this.UserId} Email:{this.UserEmail}";
        }
    }
}
