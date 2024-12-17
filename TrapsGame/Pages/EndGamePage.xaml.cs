using System;
using System.Windows;
using System.Windows.Controls;
using TrapsGame.Windows;
using TrapsGame.Processes;

namespace TrapsGame.Pages
{
    public partial class EndGamePage : Page
    {
        MainWindow _mainWindow;
        MenuPage _menuPage;

        public EndGamePage(bool isVictory, int score, TimeSpan time, MainWindow mainWindow, MenuPage menuPage)
        {
            Record.Set(score);

            _mainWindow = mainWindow;
            _menuPage = menuPage;

            InitializeComponent();

            ResultTextBlock.Text = isVictory ? "Победа!" : "Проигрыш!";

            ScoreTextBlock.Text = $"Очки: {score}";

            TimeTextBlock.Text = $"Время: {time.Minutes:00}:{time.Seconds:00}";
        }


        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangePage(_menuPage);
        }
    }
}