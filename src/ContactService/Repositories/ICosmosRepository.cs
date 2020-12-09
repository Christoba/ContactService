namespace ContactService.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Models;

    /// <summary>
    /// A contract for cosmos repositories.
    /// </summary>
    public interface ICosmosRepository
    {
        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The document.
        /// </returns>
        Task<Document> CreateAsync(UserDocument document);

        /// <summary>
        /// Creates the contact document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The document.
        /// </returns>
        Task<Document> CreateAsync(ContactDocument document);

        /// <summary>
        /// Updates the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UpdateAsync(UserDocument document);

        /// <summary>
        /// Updates the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UpdateAsync(ContactDocument document);

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteAsync(UserDocument document);

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteAsync(ContactDocument document);

        /// <summary>
        /// Gets all of the documents.
        /// </summary>
        /// <returns>
        /// The <see cref="UserDocument"/> collection.
        /// </returns>
        IEnumerable<UserDocument> GetAllUsers();

        /// <summary>
        /// Gets all contacts.
        /// </summary>
        /// <returns>
        /// The <see cref="ContactDocument"/> collection.
        /// </returns>
        IEnumerable<ContactDocument> GetAllContacts();

        /// <summary>
        /// Gets all user contacts.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The <see cref="ContactDocument"/> collection.
        /// </returns>
        IEnumerable<ContactDocument> GetAllUserContacts(string userId);

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The <see cref="UserDocument" />.
        /// </returns>
        Task<UserDocument> GetUserByEmailAsync(UserGetRequest request);

        /// <summary>
        /// Gets the contact asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The <see cref="ContactDocument"/>.
        /// </returns>
        Task<ContactDocument> GetContactAsync(ContactGetRequest request);
    }
}