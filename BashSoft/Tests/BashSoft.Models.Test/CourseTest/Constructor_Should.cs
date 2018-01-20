namespace BashSoft.Models.Test.CourseTest
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void Constructor_InitializeWithValidName_ShouldReturnName()
        {
            // Arrange & Act
            var course = new Course("Valid name");

            // Assert
            Assert.AreEqual("Valid name", course.Name);
        }

        [TestMethod]
        public void Constructor_InitializeWithNullOrEmptyName_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new Course(null));
        }

        [TestMethod]
        public void Constructor_InitializeWithEmptyName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Course(""));
        }

        [DataTestMethod]
        [DataRow(" ")]
        [DataRow("a")]
        [DataRow("This is very very very very very very very very very long name")]
        public void Constructor_InitializeInvalidLengthName_ShouldThrowArgumentException(string name)
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Course(name));
        }

        [TestMethod]
        public void Constructor_WhenInvoked_ShouldReturnInstanceOfCourse()
        {
            // Arrange & Act
            var course = new Course("Valid name");
            
            // Assert
            Assert.IsInstanceOfType(course.StudentsByName, typeof(IReadOnlyDictionary<string, IStudent>));
        }
    }
}
