using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;

namespace Talent.DataAccess.Fake.Tests
{
    [TestClass]
    public class ShowRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            FakeDatabase.Reset();
        }

        [TestMethod]
        public void ShowRepository_FetchAll_ReturnsData()
        {
            // Arrange
            var repo = new ShowRepository();

            // Act
            var results = repo.Fetch();

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 2);
            Assert.IsTrue(results.ToList()[1].Title == "Philomena");
        }

        [TestMethod]
        public void ShowRepository_FetchOne_ReturnsData()
        {
            // Arrange
            var repo = new ShowRepository();

            // Act
            var results = repo.Fetch(1);

            // Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().ShowId == 1);
            Assert.IsTrue(results.Single().Credits.Count() == 6);
            Assert.IsTrue(results.Single().ShowGenres.Count() == 1);
        }

        [TestMethod]
        public void ShowRepository_Insert_Inserts()
        {
            // Arrange
            var repo = new ShowRepository();
            var testItem = new Show
            {
                Title = "TestTitle",
                LengthInMinutes = 100,
                MpaaRatingId = 3,
                TheatricalReleaseDate = new DateTime(2000,1,31),
                DvdReleaseDate = new DateTime(2000, 4, 1)
            };
            // For ShowGenre, both ShowId and ShowGenreId should
            // be assigned by repository
            testItem.ShowGenres.Add(new ShowGenre { GenreId = 2 });

            // For Credit, both CreditId and ShowId should be
            // assigned by repository
            testItem.Credits.Add(new Credit { PersonId = 1, CreditTypeId = 1, Character = "Pooh" });

            // Act
            var insertedItem = repo.Persist(testItem);
            var newId = insertedItem.ShowId;

            // Assert
            Assert.IsTrue(newId > 0);
            var existingItem = repo.Fetch(newId).Single();
            Assert.IsTrue(existingItem.Title== "TestTitle");
            Assert.IsTrue(existingItem.LengthInMinutes == 100);
            Assert.IsTrue(existingItem.TheatricalReleaseDate == new DateTime(2000,1,31));
            Assert.IsTrue(existingItem.DvdReleaseDate == new DateTime(2000, 4, 1));
            Assert.IsTrue(existingItem.MpaaRatingId == 3);
            Assert.IsTrue(existingItem.Credits.Single().CreditId > 0);
            Assert.IsTrue(existingItem.Credits.Single().ShowId == newId);
            Assert.IsTrue(existingItem.Credits.Single().Character == "Pooh");
        }

        [TestMethod]
        public void ShowRepository_Delete_Deletes()
        {
            // Arrange
            var repo = new ShowRepository();
            var existingItem = repo.Fetch(1).Single();

            // Act
            existingItem.IsMarkedForDeletion = true;
            var deletedItem = repo.Persist(existingItem);

            // Assert for Delete
            Assert.IsNull(deletedItem);
            var emptyResult = repo.Fetch(3);
            Assert.IsFalse(emptyResult.Any());
        }

        [TestMethod]
        public void ShowRepository_Update_Updates()
        {
            /// todo: Add child updates
            // Arrange
            var repo = new ShowRepository();
            var existingItem = repo.Fetch(1).Single();

            // Act - Update
            existingItem.Title = "TestTitle";
            existingItem.LengthInMinutes = 100;
            existingItem.MpaaRatingId = 3;
            existingItem.TheatricalReleaseDate = new DateTime(2000, 1, 31);
            existingItem.DvdReleaseDate = new DateTime(2000, 4, 1);
            repo.Persist(existingItem);

            // Assert for Update
            var updatedItem = repo.Fetch(1).Single();
            Assert.IsTrue(updatedItem.Title== "TestTitle");
            Assert.IsTrue(updatedItem.LengthInMinutes == 100);
            Assert.IsTrue(updatedItem.TheatricalReleaseDate == new DateTime(2000,1,31));
            Assert.IsTrue(updatedItem.DvdReleaseDate == new DateTime(2000, 4, 1));
        }

        [TestMethod]
        public void ShowRepository_InsertUpdateDelete_Works()
        {
            // Arrange
            var repo = new ShowRepository();
            var testItem = new Show
            {
                Title = "TestTitle",
                LengthInMinutes = 100,
                MpaaRatingId = 3,
                TheatricalReleaseDate = new DateTime(2000,1,31),
                DvdReleaseDate = new DateTime(2000, 4, 1)
            };

            // Act - Insert
            var insertedItem = repo.Persist(testItem);
            var newId = insertedItem.ShowId;

            // Assert for Insert
            Assert.IsTrue(newId > 0);
            var existingItem = repo.Fetch(newId).Single();
            Assert.IsTrue(existingItem.Title== "TestTitle");
            Assert.IsTrue(existingItem.LengthInMinutes == 100);
            Assert.IsTrue(existingItem.TheatricalReleaseDate == new DateTime(2000,1,31));
            Assert.IsTrue(existingItem.DvdReleaseDate == new DateTime(2000, 4, 1));

            // Act - Update

            existingItem.Title = "TestTitle1";
            existingItem.LengthInMinutes = 101;
            existingItem.MpaaRatingId = 4;
            existingItem.TheatricalReleaseDate = new DateTime(2001, 1, 31);
            existingItem.DvdReleaseDate = new DateTime(2001, 4, 1);

            repo.Persist(existingItem);

            // Assert for Update
            var updatedItem = repo.Fetch(newId).Single();
            Assert.IsTrue(updatedItem.Title== "TestTitle1");
            Assert.IsTrue(updatedItem.LengthInMinutes == 101);
            Assert.IsTrue(updatedItem.TheatricalReleaseDate == new DateTime(2001,1,31));
            Assert.IsTrue(updatedItem.DvdReleaseDate == new DateTime(2001, 4, 1));

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
