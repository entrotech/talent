using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Talent.DataAccess.Ado.Tests
{
    [TestClass]
    public class PersonRepositoryTest
    {
        PersonRepository _personRepo;
        ShowRepository _showRepo;
        MpaaRatingRepository _ratingRepo;
        CreditTypeRepository _creditTypeRepo;

        List<MpaaRating> _ratings;
        List<CreditType> _creditTypes;
        List<Show> _shows;

        [TestInitialize]
        public void Initialize()
        {
            _showRepo = new ShowRepository();
            _ratingRepo = new MpaaRatingRepository();
            _creditTypeRepo = new CreditTypeRepository();
            _personRepo = new PersonRepository();

            _ratings = _ratingRepo.Fetch().ToList();
            _creditTypes = _creditTypeRepo.Fetch().ToList();
            _shows = _showRepo.Fetch().ToList();
        }

        /// <summary>
        /// Deletes any left-over test records.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            var _personRepo = new PersonRepository();
            var list = _personRepo.Fetch(null).ToList();

            var toDelete = list.Where(o => o.LastName == "TestLastName");
            foreach (var item in toDelete)
            {
                item.IsMarkedForDeletion = true;
                _personRepo.Persist(item);
            }
        }

        private Person CreateSamplePerson()
        {
            var newPerson = new Person
            {
                Salutation = "",
                FirstName = "Jane",
                MiddleName = "Anne",
                LastName = "TestLastName",
                Suffix = "",
                StageName = "",
                Weight = 120.5,
                DateOfBirth = new DateTime(1943, 2, 5),
                HairColorId = 1,
                EyeColorId = 1
            };

            var creditTypeRepo = new CreditTypeRepository();
            var creditTypes = creditTypeRepo.Fetch(null).ToList();

            var showRepo = new ShowRepository();
            var shows = showRepo.Fetch(null).ToList();

            Credit crd1 = new Credit
            {
                ShowId = shows[0].ShowId,
                CreditTypeId = creditTypes[0].CreditTypeId,
                Character = "Henry"
            };
            newPerson.Credits.Add(crd1);

            return newPerson;
        }

        [TestMethod]
        public void PersonRepository_FetchNull_ReturnsAll()
        {
            // Arrange
            var repo = new PersonRepository();

            var list = repo.Fetch(null);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void PersonRepository_FetchOne_ReturnsOne()
        {
            // Arrange
            var repo = new PersonRepository();
            var all = repo.Fetch(null).ToList();
            var PersonId = all[0].PersonId;
            var name = all[0].FullName;

            var item = repo.Fetch(PersonId).Single();

            Assert.IsNotNull(item);
            Assert.IsTrue(item.PersonId == PersonId);
            Assert.IsTrue(item.FullName == name);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
        }

        [TestMethod]
        public void PersonRepository_InsertDelete()
        {
            // Arrange
            var newPerson = CreateSamplePerson();

            // Act for Insert
            var item = _personRepo.Persist(newPerson);
            var newId = item.PersonId;

            // Assert for Insert - Make sure local object is updated
            Assert.IsTrue(item.PersonId > 0);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);

            // Assert for Insert - Make sure refetched object is correct
            var refetch = _personRepo.Fetch(newId).First();
            Assert.IsTrue(refetch.PersonId == newId);
            Assert.IsFalse(refetch.IsMarkedForDeletion);
            Assert.IsFalse(refetch.IsDirty);
            Assert.IsTrue(refetch.Salutation == "");
            Assert.IsTrue(refetch.FirstName == "Jane");
            Assert.IsTrue(refetch.MiddleName == "Anne");
            Assert.IsTrue(refetch.LastName == "TestLastName");
            Assert.IsTrue(refetch.Suffix == "");
            Assert.IsTrue(refetch.StageName == "");
            Assert.IsFalse(refetch.Height.HasValue);
            Assert.IsTrue(refetch.Weight == 120.5);
            Assert.IsTrue(refetch.HairColorId == 1);
            Assert.IsTrue(refetch.EyeColorId == 1);
            Assert.IsTrue(refetch.Credits.Count == 1);
            Assert.IsTrue(refetch.Credits.First().ShowId == _shows[0].ShowId);

            // Clean-up (Act for Delete)
            item.IsMarkedForDeletion = true;
            _personRepo.Persist(item);

            // Assert for Delete
            var result = _personRepo.Fetch(newId);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void PersonRepository_InsertUpdateDelete()
        {
            // Arrange
            var newPerson = CreateSamplePerson();

            // Insert test person
            var item = _personRepo.Persist(newPerson);
            var newId = item.PersonId;

            // Act for Update
            item.Salutation = "Ms";
            item.FirstName = "Jane1";
            item.MiddleName = "Anne1";
            item.LastName = "TESTLASTNAME";
            item.Suffix = "TestSuffix";
            item.StageName = "TestStageName";
            item.Height = 67;
            item.Weight = 134;
            item.HairColorId = 2;
            item.EyeColorId = 2;
            item.IsDirty = true;
            item.Credits[0].Character = "George";

            item.IsDirty = true;
            item.Credits[0].IsDirty = true;

            var updatedItem = _personRepo.Persist(item);

            Assert.IsTrue(updatedItem.IsDirty == false);
            Assert.IsTrue(updatedItem.FirstName == "Jane1");

            // Assert for Update
            var refetch = _personRepo.Fetch(newId).First();
            Assert.IsTrue(refetch.PersonId == newId);
            Assert.IsFalse(refetch.IsMarkedForDeletion);
            Assert.IsFalse(refetch.IsDirty);
            Assert.IsTrue(refetch.Salutation == "Ms");
            Assert.IsTrue(refetch.FirstName == "Jane1");
            Assert.IsTrue(refetch.MiddleName == "Anne1");
            Assert.IsTrue(refetch.LastName == "TESTLASTNAME");
            Assert.IsTrue(refetch.Suffix == "TestSuffix");
            Assert.IsTrue(refetch.StageName == "TestStageName");
            Assert.IsTrue(refetch.Height == 67);
            Assert.IsTrue(refetch.Weight == 134);
            Assert.IsTrue(refetch.HairColorId == 2);
            Assert.IsTrue(refetch.EyeColorId == 2);
            Assert.IsTrue(refetch.Credits.Count == 1);
            Assert.IsTrue(refetch.Credits[0].Character == "George");

            // Clean-up (Act for Delete)
            item.IsMarkedForDeletion = true;
            _personRepo.Persist(item);

            // Assert for Delete
            var result = _personRepo.Fetch(newId);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void PesonRepository_InvalidHairColor_ThrowsSqlException()
        {
            // Arrange
            var newPerson = CreateSamplePerson();

            try
            {
                // Act - Insert Show with an invalid HairColor
                // (Doesn't refer to an existing person id).
                newPerson.HairColorId = -1;
                var existingShow = _personRepo.Persist(newPerson);
                // Excution should not get past the above line.
                // If it does, test fails.
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SqlException));
            }
        }


        [TestMethod]
        public void PersonRepository_InvalidCredit_TransactionRollsBack()
        {
            // Arrange
            Person newPerson = CreateSamplePerson();

            // Act - Insert Person with a bad Credit member record
            // (Doesn't refer to an existing ShowId).
            newPerson.Credits[0].ShowId = -1;
            try
            {
                // Should throw exception
                var existingPerson = _personRepo.Persist(newPerson);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SqlException));
            }

            // Make sure parent show object was NOT saved.
            var savedPerson = _personRepo.Fetch()
                .Where(o => o.LastName == "TestLastName")
                .FirstOrDefault();
            Assert.IsNull(savedPerson);
        }


        #region IsGraphDirty Test

        [TestMethod]
        public void PersonRepository_CreditDirty_SetsGraphDirty()
        {
            // Arrange
            var repo = new PersonRepository();
            var all = repo.Fetch(null).ToList();
            var personId = all[0].PersonId;
            var fullName = all[0].FullName;

            var item = repo.Fetch(personId).Single();

            // Add one Credit to change a leaf
            // of the object graph

            var creditTypeRepository = new CreditTypeRepository();
            var ct = creditTypeRepository.Fetch().First();
            var showRepository = new ShowRepository();
            var s = showRepository.Fetch().First();
            var c = new Credit()
            {
                CreditTypeId = ct.CreditTypeId,
                ShowId = s.ShowId
            };

            item.Credits.Add(c);

            Assert.IsNotNull(item);
            Assert.IsTrue(item.PersonId == personId);
            Assert.IsTrue(item.FullName == fullName);
            Assert.IsFalse(item.IsMarkedForDeletion);

            // The IsDirty flag should be false
            Assert.IsFalse(item.IsDirty);

            // The HasChanges property should
            // be true, indicating the change to ShowGenres
            Assert.IsTrue(item.IsGraphDirty);
        }

        #endregion

    }
}
