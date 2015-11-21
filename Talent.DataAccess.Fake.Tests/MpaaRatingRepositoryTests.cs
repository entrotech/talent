using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;

namespace Talent.DataAccess.Fake.Tests
{
    [TestClass]
    public class MpaaRatingRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            FakeDatabase.Reset();
        }

        [TestMethod]
        public void MpaaRatingRepository_FetchAll_ReturnsData()
        {
            // Arrange
            var repo = new MpaaRatingRepository();

            // Act
            var results = repo.Fetch();

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 6);
            Assert.IsTrue(results.ToList()[5].Name == "Unrated");
        }

        [TestMethod]
        public void MpaaRatingRepository_FetchOne_ReturnsData()
        {
            // Arrange
            var repo = new MpaaRatingRepository();

            // Act
            var results = repo.Fetch(3);

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 1);
            var item = results.Single();
            Assert.IsTrue(item.MpaaRatingId == 3);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
            Assert.IsFalse(item.IsGraphDirty);
            
        }

        [TestMethod]
        public void MpaaRatingRepository_Insert_Insertss()
        {
            // Arrange
            var repo = new MpaaRatingRepository();
            var testItem = new MpaaRating
            {
                Name = "TestItem",
                Code = "TestItemCode",
                Description = "Blah",
                IsInactive = true,
                DisplayOrder = 99
            };

            // Act
            var insertedItem = repo.Persist(testItem);
            var newId = insertedItem.MpaaRatingId;

            // Assert
            Assert.IsTrue(newId > 0);
            var existingItem = repo.Fetch(newId).Single();
            Assert.IsTrue(existingItem.Name == "TestItem");
            Assert.IsTrue(existingItem.Code == "TestItemCode");
            Assert.IsTrue(existingItem.Description == "Blah");
            Assert.IsTrue(existingItem.IsInactive == true);
            Assert.IsTrue(existingItem.DisplayOrder == 99);
            Assert.IsFalse(existingItem.IsDirty);
            Assert.IsFalse(existingItem.IsMarkedForDeletion);
        }

        [TestMethod]
        public void MpaaRatingRepository_Delete_Deletes()
        {
            // Arrange
            var repo = new MpaaRatingRepository();
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
        public void MpaaRatingRepository_Update_Updates()
        {
            // Arrange
            var repo = new MpaaRatingRepository();
            var existingItem = repo.Fetch(2).Single();

            // Act - Update
            existingItem.Name = "TestItem1";
            existingItem.Code = "TestItemCode1";
            existingItem.Description = "Blah";
            existingItem.IsInactive = false;
            existingItem.DisplayOrder = 10;

            Assert.IsTrue(existingItem.IsDirty);
            Assert.IsTrue(existingItem.IsGraphDirty);

            repo.Persist(existingItem);

            // Assert for Update
            var updatedItem = repo.Fetch(2).Single();
            Assert.IsTrue(updatedItem.Name == "TestItem1");
            Assert.IsTrue(updatedItem.Code == "TestItemCode1");
            Assert.IsTrue(updatedItem.Description == "Blah");
            Assert.IsTrue(updatedItem.IsInactive == false);
            Assert.IsTrue(updatedItem.DisplayOrder == 10);
            Assert.IsFalse(existingItem.IsDirty);
            Assert.IsFalse(existingItem.IsGraphDirty);

        }

        [TestMethod]
        public void MpaaRatingRepository_InsertUpdateDelete_Works()
        {
            // Arrange
            var repo = new MpaaRatingRepository();
            var testItem = new MpaaRating
            {
                Name = "TestItem",
                Code = "TestItemCode",
                Description = "Blah",
                IsInactive = true,
                DisplayOrder = 99
            };

            // Act - Insert
            var insertedItem = repo.Persist(testItem);
            var newId = insertedItem.MpaaRatingId;

            // Assert for Insert
            Assert.IsTrue(newId > 0);
            var existingItem = repo.Fetch(newId).Single();
            Assert.IsTrue(existingItem.Name == "TestItem");
            Assert.IsTrue(existingItem.Code == "TestItemCode");
            Assert.IsTrue(existingItem.Description == "Blah");
            Assert.IsTrue(existingItem.IsInactive == true);
            Assert.IsTrue(existingItem.DisplayOrder == 99);

            // Act - Update
            existingItem.Name = "TestItem1";
            existingItem.Code = "TestItemCode1";
            existingItem.Description = "BlahBlah";
            existingItem.IsInactive = false;
            existingItem.DisplayOrder = 10;

            repo.Persist(existingItem);

            // Assert for Update
            var updatedItem = repo.Fetch(newId).Single();
            Assert.IsTrue(updatedItem.Name == "TestItem1");
            Assert.IsTrue(updatedItem.Code == "TestItemCode1");
            Assert.IsTrue(updatedItem.Description == "BlahBlah");
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
