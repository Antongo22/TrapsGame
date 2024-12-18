using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrapsGame.Processes;
using TrapsGame.Units;

namespace Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class EnemyTests
    {
        private Canvas _gameCanvas;

        [SetUp]
        public void Setup()
        {
            _gameCanvas = new Canvas { Width = 800, Height = 600 };
            Settings.Instance.EnemyWidth = 50;
            Settings.Instance.EnemyHeight = 50;
            Settings.Instance.EnemyMoveSpeed = 5;
            Settings.Instance.EnemyRandomDeviation = 0.5;
        }

        [Test]
        public void Constructor_InitializesEnemyCorrectly()
        {
            // Arrange
            double initialX = 100;
            double initialY = 100;

            // Act
            var enemy = new Enemy(_gameCanvas, initialX, initialY);

            // Assert
            Assert.AreEqual(initialX, enemy.X);
            Assert.AreEqual(initialY, enemy.Y);
            Assert.AreEqual(Settings.Instance.EnemyWidth, enemy.Width);
            Assert.AreEqual(Settings.Instance.EnemyHeight, enemy.Height);

            Assert.IsTrue(_gameCanvas.Children.Contains(enemy._enemyImage), "Enemy image should be added to the canvas.");
        }


        [Test]
        public void UpdatePosition_EnemyDoesNotExceedCanvasBounds()
        {
            // Arrange
            var enemy = new Enemy(_gameCanvas, 750, 550); 
            double playerX = 800;
            double playerY = 600;

            // Act
            enemy.UpdatePosition(playerX, playerY);

            // Assert
            Assert.LessOrEqual(enemy.X, _gameCanvas.Width - enemy.Width, "Enemy X position should not exceed canvas width.");
            Assert.LessOrEqual(enemy.Y, _gameCanvas.Height - enemy.Height, "Enemy Y position should not exceed canvas height.");
        }

        [Test]
        public void Remove_RemovesEnemyFromCanvas()
        {
            // Arrange
            var enemy = new Enemy(_gameCanvas, 100, 100);

            // Act
            enemy.Remove();

            // Assert
            Assert.IsFalse(_gameCanvas.Children.Contains(enemy._enemyImage), "Enemy should be removed from the canvas.");
        }

        [Test]
        public void UpdatePosition_EnemyMovementIncludesRandomDeviation()
        {
            // Arrange
            var enemy = new Enemy(_gameCanvas, 100, 100);
            double playerX = 200;
            double playerY = 200;

            // Act
            double previousX = enemy.X;
            double previousY = enemy.Y;
            enemy.UpdatePosition(playerX, playerY);

            // Assert
            Assert.AreNotEqual(previousX, enemy.X, "Enemy X position should change with random deviation.");
            Assert.AreNotEqual(previousY, enemy.Y, "Enemy Y position should change with random deviation.");
        }
    }
}
