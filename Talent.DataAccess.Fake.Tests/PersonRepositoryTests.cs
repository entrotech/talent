using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;

namespace Talent.DataAccess.Fake.Tests
{
    [TestClass]
    public class PersonRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            FakeDatabase.Reset();
        }

        [TestMethod]
        public void PersonRepository_FetchAll_ReturnsData()
        {
            // Arrange
            var repo = new PersonRepository();

            // Act
            var results = repo.Fetch();

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 8);
            var dench = results.ToList()[4];
            Assert.IsTrue(dench.LastName == "Dench");
            Assert.IsTrue(dench.Credits.Count() == 1);
        }

        [TestMethod]
        public void PersonRepository_FetchOne_ReturnsData()
        {
            // Arrange
            var repo = new PersonRepository();

            // Act
            var results = repo.Fetch(1);

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().PersonId == 1);
            Assert.IsTrue(results.Single().Credits.Count() == 1);
        }

        [TestMethod]
        public void PersonRepository_Insert_Inserts()
        {
            // Arrange
            var repo = new PersonRepository();
            var testItem = new Person
            {
                Salutation = "Ms",
                FirstName = "Nicole",
                MiddleName = "Ann",
                LastName = "Johnson",
                Suffix = "MBA",
                StageName = "Maybelle",
                Height = 64,
                Weight = 123,
                EyeColorId = 2,
                HairColorId = 3
            };

            // For Credit, both CreditId and PersonId should be
            // assigned by repository
            testItem.Credits.Add(new Credit { ShowId = 1, CreditTypeId = 1, Character = "Samantha" });

            // Act
            var insertedItem = repo.Persist(testItem);
            var newId = insertedItem.PersonId;

            // Assert
            Assert.IsTrue(newId > 0);
            var existingItem = repo.Fetch(newId).Single();
            Assert.IsTrue(existingItem.Salutation == "Ms");
            Assert.IsTrue(existingItem.FirstName == "Nicole");
            Assert.IsTrue(existingItem.MiddleName == "Ann");
            Assert.IsTrue(existingItem.LastName == "Johnson");
            Assert.IsTrue(existingItem.StageName == "Maybelle");
            Assert.IsTrue(existingItem.Height == 64);
            Assert.IsTrue(existingItem.Weight == 123);
            Assert.IsTrue(existingItem.EyeColorId == 2);
            Assert.IsTrue(existingItem.HairColorId == 3);
            Assert.IsTrue(existingItem.Credits.Single().CreditId > 0);
            Assert.IsTrue(existingItem.Credits.Single().PersonId == newId);
            Assert.IsTrue(existingItem.Credits.Single().Character == "Samantha");
        }

        [TestMethod]
        public void PersonRepository_Delete_Deletes()
        {
            // Arrange
            var repo = new PersonRepository();
            var existingItem = repo.Fetch(1).Single();

            // Act
            existingItem.IsMarkedForDeletion = true;
            var deletedItem = repo.Persist(existingItem);

            // Assert for Delete
            Assert.IsNull(deletedItem);
            var emptyResult = repo.Fetch(1);
            Assert.IsFalse(emptyResult.Any());
        }

        [TestMethod]
        public void PersonRepository_Update_Updates()
        {
            /// todo: Add child updates
            // Arrange
            var repo = new PersonRepository();
            var existingItem = repo.Fetch(1).Single();

            // Act - Update
            existingItem.Salutation = "Ms";
            existingItem.FirstName = "Nicole";
            existingItem.MiddleName = "Ann";
            existingItem.LastName = "Johnson";
            existingItem.Suffix = "MBA";
            existingItem.StageName = "Maybelle";
            existingItem.Height = 64;
            existingItem.Weight = 123;
            existingItem.EyeColorId = 2;
            existingItem.HairColorId = 3;
            repo.Persist(existingItem);

            // Assert for Update
            var updatedItem = repo.Fetch(1).Single();
            Assert.IsTrue(existingItem.FirstName == "Nicole");
            Assert.IsTrue(existingItem.MiddleName == "Ann");
            Assert.IsTrue(existingItem.LastName == "Johnson");
            Assert.IsTrue(existingItem.Suffix == "MBA");
            Assert.IsTrue(existingItem.StageName == "Maybelle");
            Assert.IsTrue(existingItem.Height == 64);
            Assert.IsTrue(existingItem.Weight == 123);
            Assert.IsTrue(existingItem.EyeColorId == 2);
            Assert.IsTrue(existingItem.HairColorId == 3);
        }

    }
}
