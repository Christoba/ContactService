namespace ContactService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class with utility methods for working with business logic
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// The default string length
        /// </summary>
        private const int DefaultStringLength = 6;

        /// <summary>
        /// The random backing
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Gets the user partition.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// A string representing the user partition.
        /// </returns>
        public static string GetUserPartition(UserDocument document)
        {
            const string DefaultPartition = "0";

            return !string.IsNullOrEmpty(document.LastName) ? document.LastName[0].ToString() : DefaultPartition;
        }

        /// <summary>
        /// Creates new identifier.
        /// </summary>
        /// <returns>
        /// A new identifier.
        /// </returns>
        public static string NewIdentifier()
        {
            return Guid.NewGuid().ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Generates the users.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>
        /// The users
        /// </returns>
        public static IEnumerable<UserDocument> GenerateUsers(int count)
        {
            var users = new List<UserDocument>();
            for (var i = 0; i < count; i++)
            {
                var id = NewIdentifier();

                var user = new UserDocument()
                {
                    FirstName = GenerateRandomString(),
                    LastName = GenerateRandomString(),
                    UserEmail = $"{id}@test.com",
                    UserId = id
                };

                user.UserPartition = GetUserPartition(user);

                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Generates the contacts.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// The contacts.
        /// </returns>
        public static IEnumerable<ContactDocument> GenerateContacts(string userId, int count)
        {
            var contacts = new List<ContactDocument>();
            for (var i = 0; i < count; i++)
            {
                var id = NewIdentifier();

                var contact = new ContactDocument()
                {
                    FirstName = GenerateRandomString(),
                    LastName = GenerateRandomString(),
                    ContactEmail = $"{id}@test.com",
                    ContactId = id,
                    UserId = userId
                };

                contacts.Add(contact);
            }

            return contacts;
        }

        /// <summary>
        /// Generates the random string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>
        /// The string
        /// </returns>
        private static string GenerateRandomString(int length = DefaultStringLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray()).ToLowerInvariant();
        }
    }
}
