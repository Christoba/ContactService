namespace ContactService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using Repositories;

    /// <summary>
    /// The contact service.
    /// </summary>
    public class ContactService : IContactService
    {
        /// <summary>
        /// The cosmos repository
        /// </summary>
        private readonly ICosmosRepository cosmosRepository;

        /// <summary>
        /// The log service
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactService" /> class.
        /// </summary>
        /// <param name="cosmosRepository">The cosmos repository.</param>
        /// <param name="logService">The log service.</param>
        public ContactService(ICosmosRepository cosmosRepository, ILogService logService)
        {
            this.cosmosRepository = cosmosRepository ?? throw new ArgumentNullException(nameof(cosmosRepository));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc />
        public async Task DeleteUsersAsync(CancellationToken token)
        {
            var users = this.cosmosRepository.GetAllUsers();

            var deleteTasks = users.Select(user => this.cosmosRepository.DeleteAsync(user)).ToList();

            await Task.WhenAll(deleteTasks).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteContactsAsync(CancellationToken token)
        {
            var contacts = this.cosmosRepository.GetAllContacts();

            var deleteTasks = contacts.Select(contact => this.cosmosRepository.DeleteAsync(contact)).ToList();

            await Task.WhenAll(deleteTasks).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task CreateUsersAsync(int count, CancellationToken token)
        {
            var users = DataHelper.GenerateUsers(count);

            var createTasks = users.Select(user => this.cosmosRepository.CreateAsync(user)).Cast<Task>().ToList();

            await Task.WhenAll(createTasks).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task CreateContactsAsync(string userId, int count, CancellationToken token)
        {
            var contacts = DataHelper.GenerateContacts(userId, count);

            var createTasks = contacts.Select(contact => this.cosmosRepository.CreateAsync(contact)).Cast<Task>().ToList();

            await Task.WhenAll(createTasks).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public IEnumerable<UserDocument> GetUsers(UsersGetRequest request)
        {
            return this.cosmosRepository.GetAllUsers();
        }

        /// <inheritdoc />
        public Task<UserDocument> GetUserAsync(UserGetRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.UserEmail))
            {
                throw new ContactServiceException("UserEmail is required.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            return this.cosmosRepository.GetUserByEmailAsync(request);
        }

        /// <inheritdoc />
        public Task<ContactDocument> GetContactAsync(ContactGetRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return this.cosmosRepository.GetContactAsync(request);
        }

        /// <inheritdoc />
        public IEnumerable<ContactDocument> GetContacts(ContactsGetRequest request)
        {
            return this.cosmosRepository.GetAllUserContacts(request.UserId);
        }

        /// <inheritdoc />
        public IEnumerable<ContactDocument> GetContacts()
        {
            return this.cosmosRepository.GetAllContacts();
        }

        /// <inheritdoc />
        public Task UpdateUserAsync(UserDocument user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return this.cosmosRepository.UpdateAsync(user);
        }

        /// <inheritdoc />
        public Task UpdateContactAsync(ContactDocument contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }

            return this.cosmosRepository.UpdateAsync(contact);
        }
    }
}