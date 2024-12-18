using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrapsGame.Processes;
using TrapsGame.Resources;

namespace TrapsGame.Units;

public class Enemy
{
    public readonly Rectangle _enemyImage; // Изображение врага
    private readonly Canvas _gameCanvas; // Ссылка на Canvas
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
            Width = Settings.Instance.EnemyWidth,
            Height = Settings.Instance.EnemyHeight,
            Fill = new ImageBrush { ImageSource = enemyImage },
            StrokeThickness = 2
        };

        Canvas.SetLeft(_enemyImage, X);
        Canvas.SetTop(_enemyImage, Y);

        gameCanvas.Children.Add(_enemyImage);
        Canvas.SetZIndex(_enemyImage, 1);
    }

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

        double deviationX = (_random.NextDouble() * 2 - 1) * Settings.Instance.EnemyRandomDeviation;
        double deviationY = (_random.NextDouble() * 2 - 1) * Settings.Instance.EnemyRandomDeviation;

        X += (directionX + deviationX) * Settings.Instance.EnemyMoveSpeed;
        Y += (directionY + deviationY) * Settings.Instance.EnemyMoveSpeed;

        double maxX = Math.Max(0, _gameCanvas.ActualWidth - _enemyImage.Width);
        double maxY = Math.Max(0, _gameCanvas.ActualHeight - _enemyImage.Height);

        X = Math.Clamp(X, 0, maxX);
        Y = Math.Clamp(Y, 0, maxY);

        Canvas.SetLeft(_enemyImage, X);
        Canvas.SetTop(_enemyImage, Y);
    }

    public void Remove()
    {
        _gameCanvas.Children.Remove(_enemyImage);
    }
}