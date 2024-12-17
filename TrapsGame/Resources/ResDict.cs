using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TrapsGame.Resources;

internal static class ResDict
{
    public static BitmapImage GetImage(string imageName)
    {
        // Создаем ResourceDictionary
        ResourceDictionary Resources = new ResourceDictionary()
        {
            Source = new Uri("pack://application:,,,/Resources/ResourceDictionary.xaml")
        };

        // Проверяем, содержит ли ResourceDictionary указанный ключ
        if (Resources.Contains(imageName))
        {            
            return (BitmapImage)Resources[imageName];
        }
        else
        {
            throw new Exception($"Изображение с именем '{imageName}' не найдено в ResourceDictionary.");
        }
    }
}