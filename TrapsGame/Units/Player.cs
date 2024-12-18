using System;
using System.Windows.Controls;

namespace TrapsGame.Units
{
    public class Player
    {
        private double _xPosition; // Текущая позиция по X
        private double _yPosition; // Текущая позиция по Y

        private readonly double _moveStep; // Шаг перемещения
        private readonly Image _playerImage; // Ссылка на изображение игрока
        private readonly Canvas _gameCanvas; // Ссылка на Canvas, где находится игрок

        public double X => _xPosition;
        public double Y => _yPosition;
        public double Width => _playerImage.ActualWidth;
        public double Height => _playerImage.ActualHeight;

        public Player(Image playerImage, Canvas gameCanvas, double initialX, double initialY, double moveStep)
        {
            _playerImage = playerImage;
            _gameCanvas = gameCanvas;
            _xPosition = initialX;
            _yPosition = initialY;
            _moveStep = moveStep;

            UpdateImagePosition();

            Canvas.SetZIndex(_playerImage, 2);
        }

        public void Move(bool isWPressed, bool isAPressed, bool isSPressed, bool isDPressed, TimeSpan elapsedTime)
        {
            double moveDelta = _moveStep * elapsedTime.TotalSeconds;

            if (isWPressed) _yPosition = Math.Max(0, _yPosition - moveDelta);
            if (isAPressed) _xPosition = Math.Max(0, _xPosition - moveDelta);
            if (isSPressed) _yPosition = Math.Min(_gameCanvas.ActualHeight - _playerImage.ActualHeight - 30, _yPosition + moveDelta);
            if (isDPressed) _xPosition = Math.Min(_gameCanvas.ActualWidth - _playerImage.ActualWidth, _xPosition + moveDelta);

            UpdateImagePosition();
        }

        private void UpdateImagePosition()
        {
            Canvas.SetLeft(_playerImage, _xPosition);
            Canvas.SetTop(_playerImage, _yPosition);
        }
    }
}