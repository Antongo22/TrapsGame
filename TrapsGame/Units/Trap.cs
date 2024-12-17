using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrapsGame.Resources;

namespace TrapsGame.Units;

public class Trap
{
    private readonly Rectangle _trapImage; 

    public Trap(Canvas gameCanvas, double x, double y)
    {
        BitmapImage trapImage = ResDict.GetImage("Trap");

        _trapImage = new Rectangle
        {
            Width = 30,
            Height = 30,
            Fill = new ImageBrush { ImageSource = trapImage },
            StrokeThickness = 2
        };

        Canvas.SetLeft(_trapImage, x);
        Canvas.SetTop(_trapImage, y);

        // Добавляем ловушку на Canvas
        gameCanvas.Children.Add(_trapImage);

        Canvas.SetZIndex(_trapImage, 1);
    }

    public void Remove(Canvas gameCanvas)
    {
        gameCanvas.Children.Remove(_trapImage);
    }
}