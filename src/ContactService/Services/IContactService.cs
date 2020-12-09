namespace ContactService.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    /// <summary>
    /// A contract for the contact service.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Deletes the users asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteUsersAsync(CancellationToken token);

        /// <summary>
        /// Deletes the contacts asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteContactsAsync(CancellationToken token);

        /// <summary>
        /// Creates the users asynchronous.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CreateUsersAsync(int count, CancellationToken token);

        /// <summary>
        /// Creates the contacts asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="count">The count.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CreateContactsAsync(string userId, int count, CancellationToken token);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>
        /// The <see cref="UserDocument"/> collection.
        /// </returns>
        IEnumerable<UserDocument> GetUsers(UsersGetRequest request);

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <returns>
        /// The <see cref="ContactDocument"/> collection.
        /// </returns>
        IEnumerable<ContactDocument> GetContacts();

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The <see cref="ContactDocument"/> collection.
        /// </returns>
        IEnumerable<ContactDocument> GetContacts(ContactsGetRequest request);

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The <see cref="UserDocument" />.
        /// </returns>
        Task<UserDocument> GetUserAsync(UserGetRequest request);

        /// <summary>
        /// Gets the contact asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The <see cref="ContactDocument"/>.
        /// </returns>
        Task<ContactDocument> GetContactAsync(ContactGetRequest request);

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UpdateUserAsync(UserDocument user);

        /// <summary>
        /// Updates the contact asynchronous.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UpdateContactAsync(ContactDocument contact);
    }
}