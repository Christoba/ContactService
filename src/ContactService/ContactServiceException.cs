namespace ContactService
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// The contact service exception
    /// </summary>
    [Serializable]
    public class ContactServiceException : Exception
    {
        /// <summary>
        /// The status code default
        /// </summary>
        /// <remarks>
        /// 500: Internal Server Error
        /// </remarks>
        private const int StatusCodeDefault = 500;

        /// <summary>
        /// The status code backing
        /// </summary>
        private int statusCodeBacking;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceException"/> class.
        /// </summary>
        public ContactServiceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ContactServiceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ContactServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactServiceException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ContactServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.StatusCode = info.GetInt32("StatusCode");
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode
        {
            get => this.statusCodeBacking != 0 ? this.statusCodeBacking : StatusCodeDefault;
            set => this.statusCodeBacking = value;
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("StatusCode", this.StatusCode);
            base.GetObjectData(info, context);
        }
    }
}