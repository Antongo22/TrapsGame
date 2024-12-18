using System;
using System.Windows.Controls;
using NUnit.Framework;
using TrapsGame.Processes;
using TrapsGame.Units;

namespace Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)] 
    public class GameLogicTests
    {
        private GameLogic _gameLogic;
        private Player _player;
        private Canvas _gameCanvas;

        [SetUp]
        public void Setup()
        {
            _gameCanvas = new Canvas { Width = 800, Height = 600 };

            var playerImage = new Image { Width = 50, Height = 50 };
            _player = new Player(playerImage, _gameCanvas, 100, 100, 100);

            _gameLogic = new GameLogic(_player, _gameCanvas);
        }

        [Test]
        public void Update_PlayerMoves_WhenKeysPressed()
        {
            // Act
            _gameLogic.Update(true, false, false, false, TimeSpan.FromSeconds(1));

            // Assert
            Assert.That(_player.Y, Is.LessThan(100)); 
        }

        [Test]
        public void PlaceTrap_DecreasesAvailableTraps()
        {
            // Arrange
            var initialTraps = _gameLogic.AvailableTraps;

            // Act
            _gameLogic.PlaceTrap();

            // Assert
            Assert.That(_gameLogic.AvailableTraps, Is.EqualTo(initialTraps - 1));
        }

        [Test]
        public void PlaceTrap_DoesNotPlaceTrap_WhenNoAvailableTraps()
        {
            // Arrange
            while (_gameLogic.AvailableTraps > 0)
            {
                _gameLogic.PlaceTrap();
            }

            var initialTrapsCount = _gameCanvas.Children.Count;

            // Act
            _gameLogic.PlaceTrap();

            // Assert
            Assert.That(_gameCanvas.Children.Count, Is.EqualTo(initialTrapsCount)); 
        }

        [Test]
        public void TogglePause_PausesAndResumesGame()
        {
            // Arrange
            Assert.IsFalse(_gameLogic.IsPaused);

            // Act
            _gameLogic.TogglePause();

            // Assert
            Assert.IsTrue(_gameLogic.IsPaused);

            // Act
            _gameLogic.TogglePause();

            // Assert
            Assert.IsFalse(_gameLogic.IsPaused);
        }


        [Test]
        public void DifficultyTimer_ReducesSpawnInterval()
        {
            // Arrange
            var initialInterval = _gameLogic._enemySpawnTimer.Interval.TotalSeconds;
            var expectedInterval = initialInterval * Settings.Instance.CoefficientOfIntervalReduction;

            // Act
            _gameLogic.DifficultyTimer_Tick(null, null); 
            var newInterval = _gameLogic._enemySpawnTimer.Interval.TotalSeconds;

            // Assert
            Assert.That(newInterval, Is.LessThan(initialInterval));
            Assert.That(newInterval, Is.EqualTo(expectedInterval).Within(0.0001));
        }



        [Test]
        public void OnGameEnded_InvokesGameEndedEvent()
        {
            // Arrange
            var eventInvoked = false;
            _gameLogic.GameEnded += isVictory => eventInvoked = true;

            // Act
            _gameLogic.OnGameEnded(true);

            // Assert
            Assert.IsTrue(eventInvoked);
        }

    }
}
