using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrapsGame.Resources;

namespace TrapsGame.Units;

public class Enemy
{
    private readonly Rectangle _enemyImage; // Изображение врага
    private readonly Canvas _gameCanvas; // Ссылка на Canvas
    private readonly double _moveSpeed = 1; // Основная скорость движения врага
    private readonly Random _random = new(); // Для случайного направления

    public double X { get; private set; } // Текущая позиция X
    public double Y { get; private set; } // Текущая позиция Y
    public double Width => _enemyImage.Width; // Ширина врага
    public double Height => _enemyImage.Height; // Высота врага

    public Enemy(Canvas gameCanvas, double x, double y)
    {
        _gameCanvas = gameCanvas;
        X = x;
        Y = y;

        BitmapImage enemyImage = ResDict.GetImage("Enemy");

        _enemyImage = new Rectangle
        {
            Width = 30,
            Height = 30,
            Fill = new ImageBrush { ImageSource = enemyImage },
            StrokeThickness = 2
        };

        Canvas.SetLeft(_enemyImage, X);
        Canvas.SetTop(_enemyImage, Y);

        gameCanvas.Children.Add(_enemyImage);
        Canvas.SetZIndex(_enemyImage, 1);
    }

    // Обновление позиции врага
    public void UpdatePosition(double playerX, double playerY)
    {
        double directionX = playerX - X;
        double directionY = playerY - Y;

        double distanceToPlayer = Math.Sqrt(directionX * directionX + directionY * directionY);
        if (distanceToPlayer > 0)
        {
            directionX /= distanceToPlayer;
            directionY /= distanceToPlayer;
        }

        double randomDeviation = 1; 
        double deviationX = (_random.NextDouble() * 2 - 1) * randomDeviation;
        double deviationY = (_random.NextDouble() * 2 - 1) * randomDeviation;

        X += (directionX + deviationX) * _moveSpeed;
        Y += (directionY + deviationY) * _moveSpeed;

        X = Math.Clamp(X, 0, _gameCanvas.ActualWidth - _enemyImage.Width);
        Y = Math.Clamp(Y, 0, _gameCanvas.ActualHeight - _enemyImage.Height);

        Canvas.SetLeft(_enemyImage, X);
        Canvas.SetTop(_enemyImage, Y);
    }

    public void Remove()
    {
        _gameCanvas.Children.Remove(_enemyImage);
    }
}