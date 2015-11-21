using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Ado.Tests
{
    [TestClass]
    public class EyeColorRepositoryTest 
    {

        [TestMethod]
        public void EyeColorRepository_FetchNull_ReturnsAll()
        {
            // Arrange
            var repo = new EyeColorRepository();

            var list = repo.Fetch();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void EyeColorRepository_FetchOne_ReturnsOne()
        {
            // Arrange
            var repo = new EyeColorRepository();
            var all = repo.Fetch(null).ToList();
            var eyeColorId = all[0].EyeColorId;
            var name = all[0].Name;

            var item = repo.Fetch(eyeColorId).Single();

            Assert.IsNotNull(item);
            Assert.IsTrue(item.EyeColorId == eyeColorId);
            Assert.IsTrue(item.Name == name);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
        }

     
    }
}

