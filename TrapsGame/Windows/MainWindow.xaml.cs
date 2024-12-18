using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TrapsGame.Pages;
using TrapsGame.Processes;
using TrapsGame.Resources;

namespace TrapsGame.Windows
{
    public partial class MainWindow : Window
    {
        MenuPage menuPage;
        private MediaPlayer _mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            menuPage = new MenuPage(this);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = Settings.Instance.WindowHeight;
            this.Width = Settings.Instance.WindowWidth;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string musicPath = Path.Combine(basePath, "Resources/background_music.mp3");

            
            Uri musicUri = new Uri(musicPath, UriKind.Relative);
            _mediaPlayer.Volume = Settings.Instance.MusicVolume;

            _mediaPlayer.Open(musicUri);

            _mediaPlayer.MediaEnded += (s, e) =>
            {
                _mediaPlayer.Position = TimeSpan.Zero;
                _mediaPlayer.Play();
            };

            _mediaPlayer.Play();

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