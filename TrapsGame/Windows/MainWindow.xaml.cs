using System.Windows;
using System.Windows.Controls;
using TrapsGame.Pages;
using TrapsGame.Processes;

namespace TrapsGame.Windows
{
    public partial class MainWindow : Window
    {
        MenuPage menuPage;

        public MainWindow()
        {
            menuPage = new MenuPage(this);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = Settings.Instance.WindowHeight;
            this.Width = Settings.Instance.WindowWidth;

            CenterWindowOnScreen();

            ChangePage(menuPage);
        }

        public void ChangePage(Page page)
        {
            MainFrame.Content = page;
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;

            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}