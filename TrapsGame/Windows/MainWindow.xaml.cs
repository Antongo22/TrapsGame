using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrapsGame.Pages;

namespace TrapsGame.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MenuPage menuPage;

        public const int SettingsHeight = 800;
        public const int SettingsWidth = 800;

        public MainWindow()
        {
            Height = SettingsHeight;
            Width = SettingsWidth;
            menuPage = new MenuPage(this);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChangePage(menuPage);
        }

        public void ChangePage(Page page)
        {
            MainFrame.Content = page;
        }

    }
}