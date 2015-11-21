using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Talent.Domain.Tests
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void Person_Instantiate_CreatedObject()
        {
            // Arrange
            Person p = null;

            // Act
            p = new Person();

            // Assert
            Assert.IsInstanceOfType(p, typeof(Person));
        }

        [TestMethod]
        public void Person_Instantiate_SetsDefaultProperties()
        {
            // Arrange
            Person p = null;

            // Act
            p = new Person();

            // Assert
            Assert.IsTrue(p.Salutation == String.Empty);
            Assert.IsTrue(p.FirstName == String.Empty);
            Assert.IsTrue(p.MiddleName == String.Empty);
            Assert.IsTrue(p.LastName == String.Empty);
            Assert.IsTrue(p.Suffix == String.Empty);
            Assert.IsTrue(p.StageName == String.Empty);
            Assert.IsTrue(p.DateOfBirth == null);
            Assert.IsTrue(p.Weight == null);
            Assert.IsTrue(p.Height == null);
            Assert.IsTrue(p.HairColorId == null);
            Assert.IsTrue(p.EyeColorId == null);
            Assert.IsNotNull(p.Credits);
            Assert.IsTrue(p.Credits.Count == 0);
            Assert.IsTrue(p.FirstLastName == String.Empty);
            Assert.IsTrue(p.LastFirstName == String.Empty);
            Assert.IsTrue(p.FullName == String.Empty);
            Assert.IsTrue(p.Age == null);
            Assert.IsTrue(p.ToString() == "");
        }

        [TestMethod]
        public void Person_SetStringPropertyToNull__SetsPropertyToEmptyString()
        {
            // Arrange
            Person p = null;

            // Act
            p = new Person();
            p.Salutation = null;
            p.FirstName = null;
            p.MiddleName = null;
            p.LastName = null;
            p.Suffix = null;
            p.StageName = null;

            // Assert
            Assert.IsTrue(p.Salutation == String.Empty);
            Assert.IsTrue(p.FirstName == String.Empty);
            Assert.IsTrue(p.MiddleName == String.Empty);
            Assert.IsTrue(p.LastName == String.Empty);
            Assert.IsTrue(p.Suffix == String.Empty);
            Assert.IsTrue(p.StageName == String.Empty);
       }

        [TestMethod]
        public void Person_AllNameParts_ComputedNamesAreCorrect()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                Salutation = "Mr.",
                FirstName = "George",
                MiddleName = "A",
                LastName = "Jetson",
                Suffix = "Sr."
            };

            // Assert
            Assert.IsTrue(p.FirstLastName == "George Jetson");
            Assert.IsTrue(p.LastFirstName == "Jetson, George");
            Assert.IsTrue(p.FullName == "Mr. George A Jetson, Sr.");
        }

        [TestMethod]
        public void Person_NoFirstName_ComputedNamesAreCorrect()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                Salutation = "Mr.",
                MiddleName = "A",
                LastName = "Jetson",
                Suffix = "Sr."
            };

            // Assert
            Assert.IsTrue(p.FirstLastName == "Jetson");
            Assert.IsTrue(p.LastFirstName == "Jetson");
            Assert.IsTrue(p.FullName == "Mr. A Jetson, Sr.");
        }

        [TestMethod]
        public void Person_NoLastName_ComputedNamesAreCorrect()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                Salutation = "Mr.",
                FirstName = "George",
                MiddleName = "A",
                Suffix = "Sr."
            };

            // Assert
            Assert.IsTrue(p.FirstLastName == "George");
            Assert.IsTrue(p.LastFirstName == "George");
            Assert.IsTrue(p.FullName == "Mr. George A, Sr.");
        }

        [TestMethod]
        public void Person_NoFirstOrLastName_ComputedNamesAreCorrect()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                Salutation = "Mr.",
                MiddleName = "A",
                Suffix = "Sr."
            };

            // Assert
            Assert.IsTrue(p.FirstLastName == "");
            Assert.IsTrue(p.LastFirstName == "");
            Assert.IsTrue(p.FullName == "Mr. A, Sr.");
        }

        [TestMethod]
        public void Person_OnlyFirstName_ComputedNamesAreCorrect()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                FirstName = "George"
            };

            // Assert
            Assert.IsTrue(p.FirstLastName == "George");
            Assert.IsTrue(p.LastFirstName == "George");
            Assert.IsTrue(p.FullName == "George");
        }

        [TestMethod]
        public void Person_OnlyLastName_ComputedNamesAreCorrect()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                FirstName = "Jetson"
            };

            // Assert
            Assert.IsTrue(p.FirstLastName == "Jetson");
            Assert.IsTrue(p.LastFirstName == "Jetson");
            Assert.IsTrue(p.FullName == "Jetson");
        }

        [TestMethod]
        public void Person_AgeExactly10_Returns10()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                DateOfBirth = DateTime.Today.AddYears(-10)
            };

            // Assert
            Assert.IsTrue(p.Age == 10);
        }

        [TestMethod]
        public void Person_AgeLessThan10_Returns9()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                DateOfBirth = DateTime.Today.AddYears(-10).AddDays(1)
            };

            // Assert
            Assert.IsTrue(p.Age == 9);
        }

        [TestMethod]
        public void Person_AgeLessThan0_Returns0()
        {
            // Arrange
            Person p = null;

            p = new Person()
            {
                DateOfBirth = DateTime.Today.AddYears(45)
            };

            // Assert
            Assert.IsTrue(p.Age == 0);
        }

    }
}
