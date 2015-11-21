using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Ado.Tests
{
    [TestClass]
    public class MpaaRatingRepositoryTest 
    {

        [TestMethod]
        public void MpaaRatingRepository_FetchNull_ReturnsAll()
        {
            // Arrange
            var repo = new MpaaRatingRepository();

            var list = repo.Fetch();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void MpaaRatingRepository_FetchOne_ReturnsOne()
        {
            // Arrange
            var repo = new MpaaRatingRepository();
            var all = repo.Fetch(null).ToList();
            var mpaaRatingId = all[0].MpaaRatingId;
            var name = all[0].Name;
            var description = all[0].Description;

            var item = repo.Fetch(mpaaRatingId).Single();

            Assert.IsNotNull(item);
            Assert.IsTrue(item.MpaaRatingId == mpaaRatingId);
            Assert.IsTrue(item.Name == name);
            Assert.IsTrue(item.Description == description);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
        }

        [TestMethod]
        public void MpaaRatingRepository_InsertDelete()
        {
            // Arrange
            var repo = new MpaaRatingRepository();
            var newItem = new MpaaRating
            {
                Code = "TestCode",
                Name = "TestName",
                IsInactive = false,
                DisplayOrder = 99,
                Description = "TestDescription"
            };

            // Act for Insert
            var item = repo.Persist(newItem);
            var newId = item.MpaaRatingId;

            // Assert for Insert - Make sure local object is updated
            Assert.IsTrue(item.MpaaRatingId > 0);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);

            // Assert for Insert - Make sure refetched object is correct
            var refetch = repo.Fetch(newId).First();
            Assert.IsTrue(refetch.MpaaRatingId == newId);
            Assert.IsFalse(refetch.IsMarkedForDeletion);
            Assert.IsFalse(refetch.IsDirty);
            Assert.IsTrue(refetch.Code == "TestCode");
            Assert.IsTrue(refetch.Name == "TestName");
            Assert.IsTrue(refetch.Description == "TestDescription");
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
        public void MpaaRatingRepository_InsertUpdateDelete()
        {
            // Arrange
            var repo = new MpaaRatingRepository();
            var newItem = new MpaaRating
            {
                Code = "TestCode",
                Name = "TestName",
                IsInactive = false,
                DisplayOrder = 99,
                Description = "TestDescription"
            };
            var item = repo.Persist(newItem);
            var newId = item.MpaaRatingId;

            // Act for Update
            item.Name = "XYZ";
            item.Code = "ABC";
            item.Description = "PQR";
            item.IsInactive = true;
            item.DisplayOrder = 999;
            item.IsDirty = true;
            var updatedItem = repo.Persist(item);

            Assert.IsTrue(updatedItem.IsDirty == false);
            Assert.IsTrue(updatedItem.Name == "XYZ");
            Assert.IsTrue(updatedItem.Code == "ABC");
            Assert.IsTrue(updatedItem.Description == "PQR");
            Assert.IsTrue(updatedItem.IsInactive);
            Assert.IsTrue(updatedItem.DisplayOrder == 999);

            // Assert for Update
            var refetch = repo.Fetch(newId).First();
            Assert.IsTrue(refetch.Name == "XYZ");
            Assert.IsTrue(refetch.Code == "ABC");
            Assert.IsTrue(refetch.Description == "PQR");
            Assert.IsTrue(refetch.IsInactive);
            Assert.IsTrue(refetch.DisplayOrder == 999);

            // Clean-up (Act for Delete)
            item.IsMarkedForDeletion = true;
            repo.Persist(item);

            // Assert for Delete
            var result = repo.Fetch(newId);
            Assert.IsFalse(result.Any());
        }


    }
}

