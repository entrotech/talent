using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;

namespace Talent.DataAccess.Fake.Tests
{
    [TestClass]
    public class GenreRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            FakeDatabase.Reset();
        }

        [TestMethod]
        public void GenreRepository_FetchAll_ReturnsData()
        {
            // Arrange
            var repo = new GenreRepository();

            // Act
            var results = repo.Fetch();

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 7);
            Assert.IsTrue(results.ToList()[6].Name == "Suspense");
        }

        [TestMethod]
        public void GenreRepository_FetchOne_ReturnsData()
        {
            // Arrange
            var repo = new GenreRepository();

            // Act
            var results = repo.Fetch(3);

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().GenreId == 3);
        }

        [TestMethod]
        public void GenreRepository_Insert_Insertss()
        {
            // Arrange
            var repo = new GenreRepository();
            var testItem = new Genre
            {
                Name = "TestItem",
                Code = "TestItemCode",
                IsInactive = true,
                DisplayOrder = 99
            };

            // Act
            var insertedItem = repo.Persist(testItem);
            var newId = insertedItem.GenreId;

            // Assert
            Assert.IsTrue(newId > 0);
            var existingItem = repo.Fetch(newId).Single();
            Assert.IsTrue(existingItem.Name == "TestItem");
            Assert.IsTrue(existingItem.Code == "TestItemCode");
            Assert.IsTrue(existingItem.IsInactive == true);
            Assert.IsTrue(existingItem.DisplayOrder == 99);
        }

        [TestMethod]
        public void GenreRepository_Delete_Deletes()
        {
            // Arrange
            var repo = new GenreRepository();
            var existingItem = repo.Fetch(3).Single();
            
            // Act
            existingItem.IsMarkedForDeletion = true;
            var deletedItem = repo.Persist(existingItem);

            // Assert for Delete
            Assert.IsNull(deletedItem);
            var emptyResult = repo.Fetch(3);
            Assert.IsFalse(emptyResult.Any());
        }

        [TestMethod]
        public void GenreRepository_Update_Updates()
        {
            // Arrange
            var repo = new GenreRepository();
            var existingItem = repo.Fetch(2).Single();

            // Act - Update
            existingItem.Name = "TestItem1";
            existingItem.Code = "TestItemCode1";
            existingItem.IsInactive = false;
            existingItem.DisplayOrder = 10;
            repo.Persist(existingItem);

            // Assert for Update
            var updatedItem = repo.Fetch(2).Single();
            Assert.IsTrue(updatedItem.Name == "TestItem1");
            Assert.IsTrue(updatedItem.Code == "TestItemCode1");
            Assert.IsTrue(updatedItem.IsInactive == false);
            Assert.IsTrue(updatedItem.DisplayOrder == 10);
        }

        [TestMethod]
        public void GenreRepository_InsertUpdateDelete_Works()
        {
            // Arrange
            var repo = new GenreRepository();
            var testItem = new Genre
            {
                Name = "TestItem",
                Code = "TestItemCode",
                IsInactive = true,
                DisplayOrder = 99
            };

            // Act - Insert
            var insertedItem = repo.Persist(testItem);
            var newId = insertedItem.GenreId;

            // Assert for Insert
            Assert.IsTrue(newId > 0);
            var existingItem = repo.Fetch(newId).Single();
            Assert.IsTrue(existingItem.Name == "TestItem");
            Assert.IsTrue(existingItem.Code == "TestItemCode");
            Assert.IsTrue(existingItem.IsInactive == true);
            Assert.IsTrue(existingItem.DisplayOrder == 99);

            // Act - Update

            existingItem.Name = "TestItem1";
            existingItem.Code = "TestItemCode1";
            existingItem.IsInactive = false;
            existingItem.DisplayOrder = 10;

            repo.Persist(existingItem);

            // Assert for Update
            var updatedItem = repo.Fetch(newId).Single();
            Assert.IsTrue(updatedItem.Name == "TestItem1");
            Assert.IsTrue(updatedItem.Code == "TestItemCode1");
            Assert.IsTrue(updatedItem.IsInactive == false);
            Assert.IsTrue(updatedItem.DisplayOrder == 10);

            // Act - Delete
            updatedItem.IsMarkedForDeletion = true;
            var deletedItem = repo.Persist(updatedItem);

            // Assert for Delete
            Assert.IsNull(deletedItem);
            var emptyResult = repo.Fetch(newId);
            Assert.IsFalse(emptyResult.Any());

        }
    }
}
