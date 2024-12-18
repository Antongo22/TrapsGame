using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NUnit.Framework;
using TrapsGame.Units;

namespace Tests
{
    [Apartment(ApartmentState.STA)]
    public class PlayerTests
    {
        private Player _player;
        private Canvas _gameCanvas;
        private Image _playerImage;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _gameCanvas = new Canvas { Width = 800, Height = 600 };
            _gameCanvas.Measure(new System.Windows.Size(800, 600));
            _gameCanvas.Arrange(new System.Windows.Rect(0, 0, 800, 600));

            _playerImage = new Image { Width = 50, Height = 50 };
            _playerImage.Measure(new System.Windows.Size(50, 50));
            _playerImage.Arrange(new System.Windows.Rect(0, 0, 50, 50));

            _player = new Player(_playerImage, _gameCanvas, 100, 100, 100);
        }


        [Test]
        public void Move_WhenWPressed_ShouldMoveUp()
        {
            // Arrange
            var initialY = _player.Y;

            // Act
            _player.Move(true, false, false, false, TimeSpan.FromSeconds(1));

            // Assert
            Assert.That(_player.Y, Is.LessThan(initialY));
        }

        [Test]
        public void Move_WhenAPressed_ShouldMoveLeft()
        {
            // Arrange
            var initialX = _player.X;

            // Act
            _player.Move(false, true, false, false, TimeSpan.FromSeconds(1));

            // Assert
            Assert.That(_player.X, Is.LessThan(initialX));
        }

        [Test]
        public void Move_WhenSPressed_ShouldMoveDown()
        {
            // Arrange
            var initialY = _player.Y;

            // Act
            _player.Move(false, false, true, false, TimeSpan.FromSeconds(1));

            // Assert
            Assert.That(_player.Y, Is.GreaterThan(initialY));
        }

        [Test]
        public void Move_WhenDPressed_ShouldMoveRight()
        {
            // Arrange
            var initialX = _player.X;

            // Act
            _player.Move(false, false, false, true, TimeSpan.FromSeconds(1));

            // Assert
            Assert.That(_player.X, Is.GreaterThan(initialX));
        }

        [Test]
        public void Move_WhenNoKeysPressed_ShouldNotMove()
        {
            // Arrange
            var initialX = _player.X;
            var initialY = _player.Y;

            // Act
            _player.Move(false, false, false, false, TimeSpan.FromSeconds(1));

            // Assert
            Assert.That(_player.X, Is.EqualTo(initialX));
            Assert.That(_player.Y, Is.EqualTo(initialY));
        }

        [Test]
        public void Move_WhenMovingOutOfBounds_ShouldClampPosition()
        {
            // Arrange
            _player = new Player(_playerImage, _gameCanvas, 0, 0, 100); 

            // Act
            _player.Move(true, true, false, false, TimeSpan.FromSeconds(1));

            // Assert
            Assert.That(_player.X, Is.EqualTo(0));
            Assert.That(_player.Y, Is.EqualTo(0));
        }
    }
}