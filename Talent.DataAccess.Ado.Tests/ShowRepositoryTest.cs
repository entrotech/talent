using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talent.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Talent.DataAccess.Ado.Tests
{
    [TestClass]
    public class ShowRepositoryTest
    {
        #region Fields

        ShowRepository _showRepo;
        GenreRepository _genreRepo;
        CreditTypeRepository _creditTypeRepo;
        PersonRepository _personRepo;

        List<Genre> _genres;
        List<CreditType> _creditTypes;
        List<Person> _people;

        #endregion

        #region Initialize and Cleanup

        [TestInitialize]
        public void Initialize()
        {
            _showRepo = new ShowRepository();
            _genreRepo = new GenreRepository();
            _creditTypeRepo = new CreditTypeRepository();
            _personRepo = new PersonRepository();

            _genres = _genreRepo.Fetch().ToList();
            _creditTypes = _creditTypeRepo.Fetch().ToList();
            _people = _personRepo.Fetch().ToList();
        }

        /// <summary>
        /// Deletes any left-over test records.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            var _showRepo = new ShowRepository();
            var list = _showRepo.Fetch(null).ToList();

            var toDelete = list.Where(o => o.Title == "TestTitle" || o.Title == "XYZ");
            foreach (var item in toDelete)
            {
                item.IsMarkedForDeletion = true;
                _showRepo.Persist(item);
            }
        }

        private Show CreateSampleShow()
        {
            Show show = new Show();
            show.Title = "TestTitle";
            show.LengthInMinutes = 99;
            show.TheatricalReleaseDate = new DateTime(2000, 1, 1);
            // let show.DvdReleaseDate be initialized to null

            // Add Two child ShowGenres
            show.ShowGenres.Add(new ShowGenre { GenreId = _genres[0].GenreId });
            show.ShowGenres.Add(new ShowGenre { GenreId = _genres[1].GenreId });

            // Add two child Cast members
            var credit1PersonId = _people[0].PersonId;
            Credit crd1 = new Credit
            {
                PersonId = credit1PersonId,
                CreditTypeId = _creditTypes[0].CreditTypeId,
                Character = "Henry"
            };
            show.Credits.Add(crd1);

            var credit2PersonId = _people[1].PersonId;
            Credit crd2 = new Credit
            {
                PersonId = credit2PersonId,
                CreditTypeId = _creditTypes[1].CreditTypeId,
                Character = ""
            };
            show.Credits.Add(crd2);

            return show;
        }

        #endregion

        #region Fetch Tests

        [TestMethod]
        public void ShowRepository_FetchNull_ReturnsAll()
        {
            // Arrange

            // Act
            var list = _showRepo.Fetch();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void ShowRepository_FetchOne_ReturnsOne()
        {
            // Arrange
            var all = _showRepo.Fetch().ToList();
            var ShowId = all[0].ShowId;
            var title = all[0].Title;

            var item = _showRepo.Fetch(ShowId).Single();

            Assert.IsNotNull(item);
            Assert.IsTrue(item.ShowId == ShowId);
            Assert.IsTrue(item.Title == title);
            Assert.IsFalse(item.IsMarkedForDeletion);
            Assert.IsFalse(item.IsDirty);
        }

        [TestMethod]
        public void ShowRepository_FetchNonExistent_ReturnsEmptyList()
        {
            // Arrange

            // Try to fetch non-existent Show
            var resultList = _showRepo.Fetch(-99);

            Assert.IsNotNull(resultList);
        }

        #endregion

        #region Persist Tests

        [TestMethod]
        public void ShowRepository_InsertDelete()
        {
            // Arrange
            Show newShow = CreateSampleShow();
            var genreId0 = newShow.ShowGenres[0].GenreId;
            var genreId1 = newShow.ShowGenres[1].GenreId;
            var creditPersonId0 = newShow.Credits[0].PersonId;
            var creditPersonId1 = newShow.Credits[1].PersonId;

            // Act for Insert
            var existingShow = _showRepo.Persist(newShow);
            var testShowId = existingShow.ShowId;
            // re-fetching creates a different C# Show instance
            var refetch = _showRepo.Fetch(testShowId).First();

            // Assert for Insert - Make sure local object is updated
            Assert.IsTrue(existingShow.ShowId > 0);
            Assert.IsFalse(existingShow.IsMarkedForDeletion);
            Assert.IsFalse(existingShow.IsDirty);
            Assert.IsTrue(existingShow.ShowGenres[0].ShowGenreId > 0);
            Assert.IsTrue(existingShow.ShowGenres[0].ShowGenreId > 0);
            Assert.IsTrue(existingShow.Credits[0].CreditId > 0);

            // Assert for Insert - Make sure refetched object is correct
            Assert.IsTrue(refetch.ShowId == testShowId);
            Assert.IsFalse(refetch.IsMarkedForDeletion);
            Assert.IsFalse(refetch.IsDirty);
            Assert.IsTrue(refetch.Title == "TestTitle");
            Assert.IsTrue(refetch.LengthInMinutes == 99);
            Assert.IsTrue(refetch.DvdReleaseDate == null);
            Assert.IsTrue(refetch.ShowGenres.Count() == 2);
            Assert.IsTrue(refetch.ShowGenres[0].ShowGenreId > 0);
            Assert.IsTrue(refetch.ShowGenres[1].ShowGenreId > 0);
            Assert.IsTrue(refetch.Credits.Count() == 2);
            Assert.IsTrue(refetch.Credits[0].CreditId > 0);
            Assert.IsTrue(refetch.Credits[1].CreditId > 0);

            // Clean-up (Act for Delete)
            existingShow.IsMarkedForDeletion = true;
            _showRepo.Persist(existingShow);

            // Assert for Delete
            var result = _showRepo.Fetch(testShowId);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void ShowRepository_InsertUpdateDelete()
        {
            // Arrange
            Show newShow = CreateSampleShow();
            // Record a few properties of child objects, so
            // we can find them later.
            var genreId0 = newShow.ShowGenres[0].GenreId;
            var genreId1 = newShow.ShowGenres[1].GenreId;
            var creditPersonId0 = newShow.Credits[0].PersonId;
            var creditPersonId1 = newShow.Credits[1].PersonId;

            // Act - Insert Show
            var existingShow = _showRepo.Persist(newShow);
            var testShowId = existingShow.ShowId;

            // Act for Update
            // Modify each scalar property
            existingShow.Title = "XYZ";
            existingShow.LengthInMinutes = null;
            existingShow.TheatricalReleaseDate = null;
            existingShow.DvdReleaseDate = new DateTime(2011, 3, 4);
            existingShow.IsDirty = true;

            // Delete a ShowGenre
            var showGenreForDeletion = existingShow.ShowGenres.Where(o => o.GenreId == _genres[0].GenreId).FirstOrDefault();
            showGenreForDeletion.IsMarkedForDeletion = true;
            var showGenreDeletedGenreId = showGenreForDeletion.GenreId;

            // Leave one ShowGenre unchanged
            var showGenreUnchangedGenreid = existingShow.ShowGenres[1].GenreId;

            // Insert a ShowGenre
            var showGenreInsertedGenreId = _genres[2].GenreId;
            existingShow.ShowGenres.Add(new ShowGenre { GenreId = showGenreInsertedGenreId });

            // Update a Credits member credit
            var credit0 = existingShow.Credits.Where(o => o.PersonId == creditPersonId0).First();
            credit0.Character = "George";
            // Need to set dirty flag on any modified child objects
            credit0.IsDirty = true;

            // Delete a Credits member
            var credit1 = existingShow.Credits.Where(o => o.PersonId == creditPersonId1).First();
            credit1.IsMarkedForDeletion = true;

            // Insert a Credits member
            var creditPersonId2 = _people[2].PersonId;
            Credit credit2 = new Credit
            {
                PersonId = creditPersonId2,
                CreditTypeId = _creditTypes[1].CreditTypeId,
                Character = "Samantha"
            };
            existingShow.Credits.Add(credit2);

            // Perform the update and refetch again
            // updatedItem and testShow refer to the same C# object
            existingShow = _showRepo.Persist(existingShow);

            // Make sure the show object changes are reflected in the
            // show object that remains on the client.
            Assert.IsNotNull(existingShow);
            Assert.IsNotNull(existingShow);
            Assert.IsTrue(existingShow.IsDirty == false);
            Assert.IsTrue(existingShow.IsMarkedForDeletion == false);
            Assert.IsTrue(existingShow.Title == "XYZ");
            Assert.IsTrue(existingShow.LengthInMinutes == null);
            Assert.IsTrue(existingShow.TheatricalReleaseDate == null);
            Assert.IsTrue(existingShow.DvdReleaseDate == new DateTime(2011, 3, 4));

            Assert.IsTrue(existingShow.ShowGenres.Count() == 2);

            var changedShowGenre0 = existingShow.ShowGenres
                .Where(o => o.GenreId == showGenreUnchangedGenreid)
                .FirstOrDefault();
            Assert.IsNotNull(changedShowGenre0);
            Assert.IsFalse(changedShowGenre0.IsDirty);

            var changedShowGenre1 = existingShow.ShowGenres
                .Where(o => o.GenreId == showGenreInsertedGenreId)
                .FirstOrDefault();
            Assert.IsNotNull(changedShowGenre1);
            Assert.IsFalse(changedShowGenre1.IsDirty);

            var changedShowGenre2 = existingShow.ShowGenres
                .Where(o => o.GenreId == showGenreDeletedGenreId)
                .FirstOrDefault();
            Assert.IsNull(changedShowGenre2);

            Assert.IsTrue(existingShow.Credits.Count() == 2);

            var changedCredit0 = existingShow.Credits
                .Where(o => o.PersonId == creditPersonId0).FirstOrDefault();
            Assert.IsNotNull(changedCredit0);
            Assert.IsTrue(changedCredit0.Character == "George");
            Assert.IsFalse(changedCredit0.IsDirty);

            var changedCredit1 = existingShow.Credits
                .Where(o => o.PersonId == creditPersonId1).FirstOrDefault();
            Assert.IsNull(changedCredit1);

            var changedCredit2 = existingShow.Credits
                .Where(o => o.PersonId == creditPersonId2).FirstOrDefault();
            Assert.IsNotNull(changedCredit2);
            Assert.IsFalse(changedCredit2.IsDirty);


            // re-fetching the same show from the database creates a
            // new C# object, that should be an exact replica of testShow
            var refetch = _showRepo.Fetch(testShowId).FirstOrDefault();

            // Assert - Make sure updated item has proper flags set
            Assert.IsTrue(existingShow.IsDirty == false);
            Assert.IsTrue(existingShow.Title == "XYZ");

            // Assert - Make sure re-fetched Show has expected properties
            Assert.IsNotNull(refetch);
            Assert.IsTrue(refetch.IsDirty == false);
            Assert.IsTrue(refetch.IsMarkedForDeletion == false);
            Assert.IsTrue(refetch.Title == "XYZ");
            Assert.IsTrue(refetch.LengthInMinutes == null);
            Assert.IsTrue(refetch.TheatricalReleaseDate == null);
            Assert.IsTrue(refetch.DvdReleaseDate == new DateTime(2011, 3, 4));
            Assert.IsTrue(refetch.ShowGenres.Count() == 2);
            Assert.IsTrue(refetch.ShowGenres
                .Where(o => o.GenreId == showGenreUnchangedGenreid).Count() == 1);
            Assert.IsTrue(refetch.ShowGenres
                .Where(o => o.GenreId == showGenreInsertedGenreId).Count() == 1);
            Assert.IsTrue(refetch.ShowGenres
                .Where(o => o.GenreId == showGenreDeletedGenreId).Count() == 0);
            Assert.IsTrue(refetch.Credits[0].Character == "George");

            Assert.IsTrue(refetch.Credits.Count() == 2);
            var refetchCredit0 = refetch.Credits
                .Where(o => o.PersonId == creditPersonId0).FirstOrDefault();
            var refetchCredit1 = refetch.Credits
                .Where(o => o.PersonId == creditPersonId1).FirstOrDefault();
            var refetchCredit2 = refetch.Credits
                .Where(o => o.PersonId == creditPersonId2).FirstOrDefault();
            Assert.IsTrue(refetchCredit0.Character == "George");
            Assert.IsNull(refetchCredit1);
            Assert.IsNotNull(refetchCredit2);

            // Clean-up (Act for Delete)
            existingShow.IsMarkedForDeletion = true;
            _showRepo.Persist(existingShow);

            // Assert for Delete
            var result = _showRepo.Fetch(testShowId);
            Assert.IsFalse(result.Any());
        }

        #endregion

        #region IsGraphDirty Test

        [TestMethod]
        public void ShowRepository_CreditDirty_SetsGraphDirty()
        {
            // Arrange
            var repo = new ShowRepository();
            var all = repo.Fetch(null).ToList();
            var ShowId = all[0].ShowId;
            var title = all[0].Title;

            var item = repo.Fetch(ShowId).Single();

            // Add one ShowGenre to change a leaf
            // of the object graph
            var sg = new ShowGenre();
            var genreRepository = new GenreRepository();
            var g = genreRepository.Fetch().First();
            sg.GenreId = g.GenreId;
            item.ShowGenres.Add(sg);

            Assert.IsNotNull(item);
            Assert.IsTrue(item.ShowId == ShowId);
            Assert.IsTrue(item.Title == title);
            Assert.IsFalse(item.IsMarkedForDeletion);

            // The IsDirty flag should be false
            Assert.IsFalse(item.IsDirty);

            // The IsGraphChanges property should
            // be true, indicating the change to ShowGenres
            Assert.IsTrue(item.IsGraphDirty);
        }

        #endregion

        #region Transaction Test

        [TestMethod]
        public void ShowRepository_InvalidCredit_TransactionRollsBack()
        {
            // Arrange
            Show newShow = CreateSampleShow();

            // Act - Insert Show with a bad Cast member record
            // (Doesn't refer to an existing person id).
            newShow.Credits[0].PersonId = -1;
            try
            {
                // Should throw exception
                var existingShow = _showRepo.Persist(newShow);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SqlException));
            }

            // Make sure parent show object was NOT saved.
            var savedShow = _showRepo.Fetch()
                .Where(o => o.Title == "TestTitle")
                .FirstOrDefault();
            Assert.IsNull(savedShow);
        }

        #endregion

    }
}
