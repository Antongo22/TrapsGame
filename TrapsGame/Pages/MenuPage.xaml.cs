using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrapsGame.Windows;
using TrapsGame.Processes;

namespace TrapsGame.Pages;

/// <summary>
/// Логика взаимодействия для MenuPage.xaml
/// </summary>
public partial class MenuPage : Page
{
    MainWindow _mainWindow;

    public MenuPage(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        RecordTextBlock.Text = $"Лучший результат — {Record.Get()}";
    }

    private void ButtonPlay_Click(object sender, RoutedEventArgs e)
    {
        _mainWindow.ChangePage(new GamePage(_mainWindow, this));
    }
}
