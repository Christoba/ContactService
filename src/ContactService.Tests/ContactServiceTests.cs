namespace ContactService.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Services;

    /// <summary>
    /// Tests for the contact service
    /// </summary>
    [TestFixture]
    [Category("UnitTests")]
    public class ContactServiceTests
    {
        /// <summary>
        /// The user count
        /// </summary>
        private const int UserCount = 10;

        /// <summary>
        /// The cosmos repository
        /// </summary>
        private Mock<ICosmosRepository> cosmosRepository;

        /// <summary>
        /// The log service
        /// </summary>
        private Mock<ILogService> logService;

        /// <summary>
        /// The contact service
        /// </summary>
        private IContactService service;

        /// <summary>
        /// The message results
        /// </summary>
        private IEnumerable<UserDocument> userResults;

        /// <summary>
        /// The users
        /// </summary>
        private List<UserDocument> users;

        /// <summary>
        /// The created user
        /// </summary>
        private UserDocument createdUser;

        /// <summary>
        /// The user result
        /// </summary>
        private UserDocument userResult;

        /// <summary>
        /// The deleted user count
        /// </summary>
        private int deletedUserCount;

        /// <summary>
        /// Setup this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.deletedUserCount = 0;
        }

        /// <summary>
        /// Should get all users.
        /// </summary>
        [Test]
        public void ShouldGetAllUsers()
        {
            this.GivenUserDocuments();
            this.GivenTheCosmosRepository();
            this.GivenTheContactService();
            this.WhenGettingAllUsers();
            this.ThenTheUsersAreReturned();
        }

        /// <summary>
        /// Should get the user.
        /// </summary>
        [Test]
        public async Task ShouldGetUser()
        {
            this.GivenUserDocuments();
            this.GivenTheCosmosRepository();
            this.GivenTheContactService();
            await this.WhenGettingAUserAsync().ConfigureAwait(false);
            this.ThenTheUserIsReturned();
        }

        [Test]
        public async Task ShouldDeleteUsers()
        {
            this.GivenUserDocuments();
            this.GivenTheCosmosRepository();
            this.GivenTheContactService();
            await this.WhenDeletingUsersAsync().ConfigureAwait(false);
            this.ThenTheUsersAreDeleted();
        }

        /// <summary>
        /// Then the documents match.
        /// </summary>
        /// <param name="list1">The list1.</param>
        /// <param name="list2">The list2.</param>
        private static void ThenTheDocumentsMatch(IReadOnlyCollection<UserDocument> list1, IReadOnlyCollection<UserDocument> list2)
        {
            foreach (var document in list1)
            {
                var match = list2.FirstOrDefault(x =>
                    x.UserId == document.UserId);

                Assert.IsNotNull(match);
                Assert.AreEqual(document.UserPartition, match.UserPartition);
                Assert.AreEqual(document.UserEmail, match.UserEmail);
                Assert.AreEqual(document.LastName, match.LastName);
                Assert.AreEqual(document.FirstName, match.FirstName);
            }
        }

        /// <summary>
        /// Given the cosmos repository.
        /// </summary>
        private void GivenTheCosmosRepository()
        {
            this.cosmosRepository = new Mock<ICosmosRepository>();
            this.cosmosRepository.Setup(x => x.GetAllUsers()).Returns(this.users);
            this.cosmosRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<UserGetRequest>()))
                .ReturnsAsync(this.createdUser);
            this.cosmosRepository.Setup(x => x.DeleteAsync(It.IsAny<UserDocument>()))
                .Callback((UserDocument a) => this.deletedUserCount++).Returns(Task.CompletedTask);
        }

        /// <summary>
        /// Given the document service.
        /// </summary>
        private void GivenTheContactService()
        {
            this.logService = new Mock<ILogService>();
            this.service = new ContactService(this.cosmosRepository.Object, this.logService.Object);
        }

        /// <summary>
        /// Given the users.
        /// </summary>
        private void GivenUserDocuments()
        {
            this.users = DataHelper.GenerateUsers(UserCount).ToList();
            this.createdUser = this.users.First();
        }

        /// <summary>
        /// When getting all users.
        /// </summary>
        private void WhenGettingAllUsers()
        {
            this.userResults = this.service.GetUsers(new UsersGetRequest());
        }

        /// <summary>
        /// When getting a user asynchronous.
        /// </summary>
        private async Task WhenGettingAUserAsync()
        {
            this.userResult = await this.service.GetUserAsync(new UserGetRequest()
                    {UserEmail = this.createdUser.UserEmail, UserPartition = this.createdUser.UserPartition})
                .ConfigureAwait(false);
        }

        /// <summary>
        /// When deleting users asynchronous.
        /// </summary>
        private async Task WhenDeletingUsersAsync()
        {
            await this.service.DeleteUsersAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Then users are returned
        /// </summary>
        private void ThenTheUsersAreReturned()
        {
            Assert.AreEqual(this.users.Count(), this.userResults.Count());

            ThenTheDocumentsMatch(this.users, this.userResults.ToList());
        }

        /// <summary>
        /// Then the user is returned.
        /// </summary>
        private void ThenTheUserIsReturned()
        {
            Assert.AreEqual(this.createdUser.UserPartition, this.userResult.UserPartition);
            Assert.AreEqual(this.createdUser.UserEmail, this.userResult.UserEmail);
            Assert.AreEqual(this.createdUser.LastName, this.userResult.LastName);
            Assert.AreEqual(this.createdUser.FirstName, this.userResult.FirstName);
        }

        /// <summary>
        /// Then the users are deleted.
        /// </summary>
        private void ThenTheUsersAreDeleted()
        {
            Assert.AreEqual(this.deletedUserCount, UserCount);
        }
    }
}