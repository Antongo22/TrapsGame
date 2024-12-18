using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NUnit.Framework;
using TrapsGame.Processes;
using TrapsGame.Units;

namespace Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TrapTests
    {
        private Canvas _testCanvas;

        [SetUp]
        public void SetUp()
        {
            _testCanvas = new Canvas();
        }

        [Test]
        public void Trap_InitializesCorrectly()
        {
            // Arrange
            double expectedX = 100;
            double expectedY = 200;

            // Act
            var trap = new Trap(_testCanvas, expectedX, expectedY);

            // Assert
            Assert.AreEqual(expectedX, trap.X, "Trap X coordinate is incorrect");
            Assert.AreEqual(expectedY, trap.Y, "Trap Y coordinate is incorrect");
            Assert.AreEqual(Settings.Instance.TrapWidth, trap.Width, "Trap width is incorrect");
            Assert.AreEqual(Settings.Instance.TrapHeight, trap.Height, "Trap height is incorrect");

            Assert.IsTrue(_testCanvas.Children.Contains((Rectangle)trap.GetType()
                .GetField("_trapImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .GetValue(trap)),
                "Trap image is not added to the Canvas");
        }

        [Test]
        public void Trap_Remove_RemovesFromCanvas()
        {
            // Arrange
            var trap = new Trap(_testCanvas, 100, 200);

            // Act
            trap.Remove();

            // Assert
            Assert.IsFalse(_testCanvas.Children.Contains((Rectangle)trap.GetType()
                .GetField("_trapImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .GetValue(trap)),
                "Trap image was not removed from the Canvas");
        }

        [Test]
        public void Trap_ZIndex_IsCorrect()
        {
            // Arrange
            var trap = new Trap(_testCanvas, 100, 200);

            // Act
            var zIndex = Canvas.GetZIndex((Rectangle)trap.GetType()
                .GetField("_trapImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .GetValue(trap));

            // Assert
            Assert.AreEqual(1, zIndex, "Trap ZIndex is incorrect");
        }
    }
}
