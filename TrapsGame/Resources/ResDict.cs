using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TrapsGame.Resources;

public static class ResDict
{
    public static BitmapImage GetImage(string imageName)
    {
        try
        {
            ResourceDictionary Resources = new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/Resources/ResourceDictionary.xaml")
            };

            if (Resources.Contains(imageName))
            {
                return (BitmapImage)Resources[imageName];
            }
            else
            {
                throw new Exception($"Изображение с именем '{imageName}' не найдено в ResourceDictionary.");
            }

        }
        catch
        {
            return null;
        }
    }
}