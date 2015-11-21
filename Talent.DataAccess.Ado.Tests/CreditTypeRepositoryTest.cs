using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Ado.Tests
{
    [TestClass]
    public class CreditTypeRepositoryTest 
    {

        [TestMethod]
        public void CreditTypeRepository_FetchNull_ReturnsAll()
        {
            // Arrange
            var repo = new CreditTypeRepository();

            var list = repo.Fetch();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void CreditTypeRepository_FetchOne_ReturnsOne()
        {
            // Arrange
            var repo = new CreditTypeRepository();
            var all = repo.Fetch(null).ToList();
            var creditTypeId = all[0].CreditTypeId;
            var name = all[0].Name;

            var item = repo.Fetch(creditTypeId).Single();

            Assert.IsNotNull(item);
            Assert.IsTrue(item.CreditTypeId == creditTypeId);
            Assert.IsTrue(item.Name == name);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
        }

        [TestMethod]
        public void CreditTypeRepository_InsertDelete()
        {
            // Arrange
            var repo = new CreditTypeRepository();
            var newItem = new CreditType
            {
                Code = "TestCode",
                Name = "TestName",
                IsInactive = false,
                DisplayOrder = 99
            };

            // Act for Insert
            var item = repo.Persist(newItem);
            var newId = item.CreditTypeId;

            // Assert for Insert - Make sure local object is updated
            Assert.IsTrue(item.CreditTypeId > 0);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);

            // Assert for Insert - Make sure refetched object is correct
            var refetch = repo.Fetch(newId).First();
            Assert.IsTrue(refetch.CreditTypeId == newId);
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
        public void CreditTypeRepository_InsertUpdateDelete()
        {
            // Arrange
            var repo = new CreditTypeRepository();
            var newItem = new CreditType
            {
                Code = "TestCode",
                Name = "TestName",
                IsInactive = false,
                DisplayOrder = 99
            };
            var item = repo.Persist(newItem);
            var newId = item.CreditTypeId;

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

