using System;
using System.IO;
using System.Windows.Media.Media3D;
using NUnit.Framework;
using TrapsGame.Processes;

namespace Tests
{
    [TestFixture]
    public class RecordTests
    {
        private string _testPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Path.Combine(Directory.GetCurrentDirectory(), "test_record.txt");
            typeof(Record).GetField("_path", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, _testPath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testPath))
            {
                File.Delete(_testPath);
            }
        }

        [Test]
        public void Get_CreatesFile_WhenFileDoesNotExist()
        {
            // Arrange
            Assert.IsFalse(File.Exists(_testPath), "Test file should not exist initially");

            // Act
            int record = Record.Get();

            // Assert
            Assert.IsTrue(File.Exists(_testPath), "File was not created");
            Assert.AreEqual(0, record, "Default record value should be 0");
        }

        [Test]
        public void Get_ReturnsValueFromExistingFile()
        {
            // Arrange
            File.WriteAllText(_testPath, "42");

            // Act
            int record = Record.Get();

            // Assert
            Assert.AreEqual(42, record, "Record value should match the value in the file");
        }

        [Test]
        public void Get_ReturnsDefaultValue_WhenFileContainsInvalidData()
        {
            // Arrange
            File.WriteAllText(_testPath, "invalid");

            // Act
            int record = Record.Get();

            // Assert
            Assert.AreEqual(0, record, "Record value should default to 0 when file contains invalid data");
        }

        [Test]
        public void Set_UpdatesRecord_WhenNewValueIsHigher()
        {
            // Arrange
            File.WriteAllText(_testPath, "50");

            // Act
            Record.Set(100);

            // Assert
            string updatedValue = File.ReadAllText(_testPath);
            Assert.AreEqual("100", updatedValue, "Record value should update to the new higher value");
        }

        [Test]
        public void Set_DoesNotUpdateRecord_WhenNewValueIsLower()
        {
            // Arrange
            File.WriteAllText(_testPath, "50");

            // Act
            Record.Set(25);

            // Assert
            string updatedValue = File.ReadAllText(_testPath);
            Assert.AreEqual("50", updatedValue, "Record value should not update if the new value is lower");
        }
    }
}
