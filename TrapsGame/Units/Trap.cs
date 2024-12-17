using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrapsGame.Processes;
using TrapsGame.Resources;

namespace TrapsGame.Units;

public class Trap
{
    private readonly Rectangle _trapImage; // Изображение ловушки
    private readonly Canvas _gameCanvas; // Ссылка на Canvas


    public double X { get; } // Позиция X ловушки
    public double Y { get; } // Позиция Y ловушки
    public double Width => _trapImage.Width; // Ширина ловушки
    public double Height => _trapImage.Height; // Высота ловушки



    public Trap(Canvas gameCanvas, double x, double y)
    {
        _gameCanvas = gameCanvas;
        X = x;
        Y = y;

        BitmapImage trapImage = ResDict.GetImage("Trap");

        _trapImage = new Rectangle
        {
            Width = Settings.Instance.TrapWidth,
            Height = Settings.Instance.TrapHeight,
            Fill = new ImageBrush { ImageSource = trapImage },
            Stroke = Brushes.Black,
            StrokeThickness = Settings.Instance.TrapStrokeThickness
        };

        Canvas.SetLeft(_trapImage, X);
        Canvas.SetTop(_trapImage, Y);

        gameCanvas.Children.Add(_trapImage);

        Canvas.SetZIndex(_trapImage, 1);
    }

    public void Remove()
    {
        _gameCanvas.Children.Remove(_trapImage);
    }
}