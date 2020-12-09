namespace ContactService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Models;

    /// <summary>
    /// A repository for working with Cosmos documents.
    /// </summary>
    public class CosmosRepository : ICosmosRepository
    {
        /// <summary>
        /// The maximum per feed request
        /// </summary>
        private const int MaxPerFeedRequest = 100;

        /// <summary>
        /// The client
        /// </summary>
        private readonly IDocumentClient client;

        /// <summary>
        /// The user connection information
        /// </summary>
        private readonly ICosmosConnectionInfo userConnectionInfo;

        /// <summary>
        /// The contact connection information
        /// </summary>
        private readonly ICosmosConnectionInfo contactConnectionInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosRepository" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="userConnectionInfo">The user connection information.</param>
        /// <param name="contactConnectionInfo">The contact connection information.</param>
        public CosmosRepository(
            IDocumentClient client,
            ICosmosConnectionInfo userConnectionInfo,
            ICosmosConnectionInfo contactConnectionInfo)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.userConnectionInfo = userConnectionInfo ?? throw new ArgumentNullException(nameof(userConnectionInfo));
            this.contactConnectionInfo = contactConnectionInfo ?? throw new ArgumentNullException(nameof(contactConnectionInfo));
        }

        /// <summary>
        /// Gets the user document collection URI.
        /// </summary>
        /// <value>
        /// The user document collection URI.
        /// </value>
        private Uri UserDocumentCollectionUri =>
            UriFactory.CreateDocumentCollectionUri(this.userConnectionInfo.DatabaseId, this.userConnectionInfo.CollectionId);

        /// <summary>
        /// Gets the contact document collection URI.
        /// </summary>
        /// <value>
        /// The contact document collection URI.
        /// </value>
        private Uri ContactDocumentCollectionUri =>
            UriFactory.CreateDocumentCollectionUri(this.contactConnectionInfo.DatabaseId, this.contactConnectionInfo.CollectionId);

        /// <inheritdoc />
        public async Task<Document> CreateAsync(UserDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var response = await this.client.CreateDocumentAsync(this.UserDocumentCollectionUri, document)
                .ConfigureAwait(false);

            return response.Resource;
        }

        /// <inheritdoc />
        public async Task<Document> CreateAsync(ContactDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var response = await this.client.CreateDocumentAsync(this.ContactDocumentCollectionUri, document)
                .ConfigureAwait(false);

            return response.Resource;
        }

        /// <inheritdoc />
        public Task UpdateAsync(UserDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var documentLink = this.CreateUserDocumentUri(document.Id);
            return this.client.ReplaceDocumentAsync(
                documentLink,
                document,
                new RequestOptions() { PartitionKey = new PartitionKey(document.UserPartition) });
        }

        /// <inheritdoc />
        public Task UpdateAsync(ContactDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var documentLink = this.CreateContactDocumentUri(document.Id);
            return this.client.ReplaceDocumentAsync(
                documentLink,
                document,
                new RequestOptions() { PartitionKey = new PartitionKey(document.UserId) });
        }

        /// <inheritdoc />
        public Task DeleteAsync(UserDocument document)
        {
            var documentLink = this.CreateUserDocumentUri(document.Id);

            return this.client.DeleteDocumentAsync(documentLink,
                new RequestOptions() {PartitionKey = new PartitionKey(document.UserPartition)});
        }

        /// <inheritdoc />
        public Task DeleteAsync(ContactDocument document)
        {
            var documentLink = this.CreateContactDocumentUri(document.Id);

            return this.client.DeleteDocumentAsync(documentLink,
                new RequestOptions() { PartitionKey = new PartitionKey(document.UserId) });
        }

        /// <inheritdoc />
        public IEnumerable<UserDocument> GetAllUsers()
        {
            var documents = this.client.CreateDocumentQuery<UserDocument>(
                this.UserDocumentCollectionUri,
                new FeedOptions()
                    {
                        ConsistencyLevel = ConsistencyLevel.Session,
                        MaxItemCount = MaxPerFeedRequest,
                        EnableCrossPartitionQuery = true
                }).AsEnumerable();

            return documents;
        }

        /// <inheritdoc />
        public IEnumerable<ContactDocument> GetAllContacts()
        {
            var documents = this.client.CreateDocumentQuery<ContactDocument>(
                this.ContactDocumentCollectionUri,
                new FeedOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Session,
                    MaxItemCount = MaxPerFeedRequest,
                    EnableCrossPartitionQuery = true
                }).AsEnumerable();

            return documents;
        }

        /// <inheritdoc />
        public async Task<UserDocument> GetUserByEmailAsync(UserGetRequest request)
        {
            var crossPartition = string.IsNullOrWhiteSpace(request.UserPartition);

            var query = this.client.CreateDocumentQuery<UserDocument>(
                this.UserDocumentCollectionUri,
                new FeedOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Session,
                    MaxItemCount = MaxPerFeedRequest,
                    EnableCrossPartitionQuery = crossPartition,
                    PartitionKey = crossPartition ? null : new PartitionKey(request.UserPartition)
                }).Where(x => x.UserEmail == request.UserEmail).AsDocumentQuery();

            if (!query.HasMoreResults)
            {
                return null;
            }

            var documentFeed = await query.ExecuteNextAsync<UserDocument>().ConfigureAwait(false);
            return documentFeed.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<ContactDocument> GetContactAsync(ContactGetRequest request)
        {
            var query = this.client.CreateDocumentQuery<ContactDocument>(
                this.ContactDocumentCollectionUri,
                new FeedOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Session,
                    MaxItemCount = MaxPerFeedRequest,
                    PartitionKey = new PartitionKey(request.UserId)
                }).Where(x => x.ContactId == request.ContactId).AsDocumentQuery();

            if (!query.HasMoreResults)
            {
                return null;
            }

            var documentFeed = await query.ExecuteNextAsync<ContactDocument>().ConfigureAwait(false);
            return documentFeed.FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<ContactDocument> GetAllUserContacts(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var documents = this.client.CreateDocumentQuery<ContactDocument>(
                this.ContactDocumentCollectionUri,
                new FeedOptions()
                {
                    ConsistencyLevel = ConsistencyLevel.Session,
                    MaxItemCount = MaxPerFeedRequest,
                    PartitionKey = new PartitionKey(userId)
                }).AsEnumerable();

            return documents;
        }

        /// <summary>
        /// Creates the document URI.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        private Uri CreateUserDocumentUri(string documentId)
        {
            return UriFactory.CreateDocumentUri(
                this.userConnectionInfo.DatabaseId,
                this.userConnectionInfo.CollectionId,
                documentId);
        }

        /// <summary>
        /// Creates the contact document URI.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        private Uri CreateContactDocumentUri(string documentId)
        {
            return UriFactory.CreateDocumentUri(
                this.contactConnectionInfo.DatabaseId,
                this.contactConnectionInfo.CollectionId,
                documentId);
        }
    }
}