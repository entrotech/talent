using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Talent.Domain.Tests
{
    [TestClass]
    public class ShowTest
    {
        [TestMethod]
        public void Show_Instantiate_CreatesObject()
        {
            // Arrange
            Show s = null;

            //Act
            s = new Show();

            // Assert
            Assert.IsInstanceOfType(s, typeof(Show));
        }

        [TestMethod]
        public void Show_Instantiate_SetsPublicProperties()
        {
            // Arrange
            Show s = null;

            // Act
            s = new Show();

            // Assert
            Assert.IsTrue(s.Title == String.Empty);
            Assert.IsTrue(s.LengthInMinutes.HasValue == false);
            Assert.IsTrue(s.TheatricalReleaseDate.HasValue == false);
            Assert.IsTrue(s.DvdReleaseDate.HasValue == false);
            Assert.IsTrue(s.MpaaRatingId == null);
            Assert.IsNotNull(s.ShowGenres);
            Assert.IsTrue(s.ShowGenres.Count == 0);
            Assert.IsNotNull(s.Credits);
            Assert.IsTrue(s.Credits.Count == 0);
            Assert.IsTrue(s.ToString() == String.Empty);  

        }

        [TestMethod]
        public void Show_ToString_ReturnsTitle()
        {
            // Arrange
            Show s = null;
            string testTitle = "The Wizard Of Oz";

            // Act
            s = new Show() { Title = testTitle };

            // Assert
            Assert.IsTrue(s.ToString() == testTitle);

        }
    }
}
