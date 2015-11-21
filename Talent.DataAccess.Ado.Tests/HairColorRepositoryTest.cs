using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Ado.Tests
{
    [TestClass]
    public class HairColorRepositoryTest 
    {

        [TestMethod]
        public void HairColorRepository_FetchNull_ReturnsAll()
        {
            // Arrange
            var repo = new HairColorRepository();

            var list = repo.Fetch();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void HairColorRepository_FetchOne_ReturnsOne()
        {
            // Arrange
            var repo = new HairColorRepository();
            var all = repo.Fetch(null).ToList();
            var hairColorId = all[0].HairColorId;
            var name = all[0].Name;

            var item = repo.Fetch(hairColorId).Single();

            Assert.IsNotNull(item);
            Assert.IsTrue(item.HairColorId == hairColorId);
            Assert.IsTrue(item.Name == name);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
        }

    }
}

