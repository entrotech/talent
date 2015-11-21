using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Talent.Domain;

namespace Talent.DataAccess.Ado.Tests
{
    [TestClass]
    public class GenreRepositoryTest
    {
        [TestMethod]
        public void GenreRepository_FetchNull_ReturnsAll()
        {
            // Arrange
            var repo = new GenreRepository();

            var list = repo.Fetch();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void GenreRepository_FetchOne_ReturnsOne()
        {
            // Arrange
            var repo = new GenreRepository();
            var all = repo.Fetch(null).ToList();
            var genreId = all[0].GenreId;
            var name = all[0].Name;

            var item = repo.Fetch(genreId).Single();

            Assert.IsNotNull(item);
            Assert.IsTrue(item.GenreId == genreId);
            Assert.IsTrue(item.Name == name);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
        }

        [TestMethod]
        public void GenreRepository_InsertDelete()
        {
            // Arrange
            var repo = new GenreRepository();
            var newItem = new Genre
            {
                Code = "TestCode",
                Name = "TestName",
                IsInactive = false,
                DisplayOrder = 99
            };

            // Act for Insert
            var item = repo.Persist(newItem);
            var newId = item.GenreId;

            // Assert for Insert - Make sure local object is updated
            Assert.IsTrue(item.GenreId > 0);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);

            // Assert for Insert - Make sure refetched object is correct
            var refetch = repo.Fetch(newId).First();
            Assert.IsTrue(refetch.GenreId == newId);
            Assert.IsFalse(refetch.IsMarkedForDeletion);
            Assert.IsFalse(refetch.IsDirty);
            Assert.IsTrue(refetch.Code == "TestCode");
            Assert.IsTrue(refetch.Name == "TestName");
            Assert.IsTrue(refetch.IsInactive == false);
            Assert.IsTrue(refetch.DisplayOrder == 99);

            // Clean-up (Act for Delete)
            item.IsMarkedForDeletion = true;
            repo.Persist(item);

            // Assert for Delete
            var result = repo.Fetch(newId);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GenreRepository_InsertUpdateDelete()
        {
            // Arrange
            var repo = new GenreRepository();
            var newItem = new Genre
            {
                Code = "TestCode",
                Name = "TestName",
                IsInactive = false,
                DisplayOrder = 99
            };
            var item = repo.Persist(newItem);
            var newId = item.GenreId;

            // Act for Update
            item.Name = "XYZ";
            item.Code = "ABC";
            item.IsInactive = true;
            item.DisplayOrder = 999;
            item.IsDirty = true;
            var updatedItem = repo.Persist(item);

            Assert.IsTrue(updatedItem.IsDirty == false);
            Assert.IsTrue(updatedItem.Name == "XYZ");
            Assert.IsTrue(updatedItem.Code == "ABC");
            Assert.IsTrue(updatedItem.IsInactive);
            Assert.IsTrue(updatedItem.DisplayOrder == 999);

            // Assert for Update
            var refetch = repo.Fetch(newId).First();
            Assert.IsTrue(refetch.Name == "XYZ");

            // Clean-up (Act for Delete)
            item.IsMarkedForDeletion = true;
            repo.Persist(item);

            // Assert for Delete
            var result = repo.Fetch(newId);
            Assert.IsFalse(result.Any());
        }



    }
}
