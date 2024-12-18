using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TrapsGame.Processes;

namespace TrapsGame.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            var settings = Settings.Instance;

            InitialAvailableTrapsTextBox.Text = settings.InitialAvailableTraps.ToString();
            InitialSpawnIntervalTextBox.Text = settings.InitialSpawnInterval.ToString(CultureInfo.InvariantCulture);
            MinSpawnIntervalTextBox.Text = settings.MinSpawnInterval.ToString(CultureInfo.InvariantCulture);
            ScorePerSecondTextBox.Text = settings.ScorePerSecond.ToString();
            ScorePerEnemyTextBox.Text = settings.ScorePerEnemy.ToString();
            VictoryTimeTextBox.Text = settings.VictoryTime.ToString();
            PlayerMoveStepTextBox.Text = settings.PlayerMoveStep.ToString(CultureInfo.InvariantCulture);
            EnemySpawnDistanceTextBox.Text = settings.EnemySpawnDistance.ToString(CultureInfo.InvariantCulture);
            CoefficientOfIntervalReductionTextBox.Text = settings.CoefficientOfIntervalReduction.ToString(CultureInfo.InvariantCulture);
            TimeIntervalForTheAppearanceOfEnemiesTextBox.Text = settings.TimeIntervalForTheAppearanceOfEnemies.ToString(CultureInfo.InvariantCulture);
            PlayerStartPositionXTextBox.Text = settings.PlayerStartPositionX.ToString(CultureInfo.InvariantCulture);
            PlayerStartPositionYTextBox.Text = settings.PlayerStartPositionY.ToString(CultureInfo.InvariantCulture);
            TrapWidthTextBox.Text = settings.TrapWidth.ToString(CultureInfo.InvariantCulture);
            TrapHeightTextBox.Text = settings.TrapHeight.ToString(CultureInfo.InvariantCulture);
            TrapStrokeThicknessTextBox.Text = settings.TrapStrokeThickness.ToString(CultureInfo.InvariantCulture);
            EnemyWidthTextBox.Text = settings.EnemyWidth.ToString(CultureInfo.InvariantCulture);
            EnemyHeightTextBox.Text = settings.EnemyHeight.ToString(CultureInfo.InvariantCulture);
            EnemyMoveSpeedTextBox.Text = settings.EnemyMoveSpeed.ToString(CultureInfo.InvariantCulture);
            EnemyRandomDeviationTextBox.Text = settings.EnemyRandomDeviation.ToString(CultureInfo.InvariantCulture);
            WindowWidthTextBox.Text = settings.WindowWidth.ToString(CultureInfo.InvariantCulture);
            WindowHeightTextBox.Text = settings.WindowHeight.ToString(CultureInfo.InvariantCulture);
            PlayerWidthTextBox.Text = settings.PlayerWidth.ToString(CultureInfo.InvariantCulture);
            PlayerHeightTextBox.Text = settings.PlayerHeight.ToString(CultureInfo.InvariantCulture);
            MusicVolumeSlider.Value = settings.MusicVolume;
            MusicVolumeTextBox.Text = settings.MusicVolume.ToString(CultureInfo.InvariantCulture);
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = Settings.Instance;

                settings.InitialAvailableTraps = int.Parse(InitialAvailableTrapsTextBox.Text);
                settings.InitialSpawnInterval = double.Parse(InitialSpawnIntervalTextBox.Text, CultureInfo.InvariantCulture);
                settings.MinSpawnInterval = double.Parse(MinSpawnIntervalTextBox.Text, CultureInfo.InvariantCulture);
                settings.ScorePerSecond = int.Parse(ScorePerSecondTextBox.Text);
                settings.ScorePerEnemy = int.Parse(ScorePerEnemyTextBox.Text);
                settings.VictoryTime = int.Parse(VictoryTimeTextBox.Text);
                settings.PlayerMoveStep = double.Parse(PlayerMoveStepTextBox.Text, CultureInfo.InvariantCulture);
                settings.EnemySpawnDistance = double.Parse(EnemySpawnDistanceTextBox.Text, CultureInfo.InvariantCulture);
                settings.CoefficientOfIntervalReduction = double.Parse(CoefficientOfIntervalReductionTextBox.Text, CultureInfo.InvariantCulture);
                settings.TimeIntervalForTheAppearanceOfEnemies = double.Parse(TimeIntervalForTheAppearanceOfEnemiesTextBox.Text, CultureInfo.InvariantCulture);
                settings.PlayerStartPositionX = double.Parse(PlayerStartPositionXTextBox.Text, CultureInfo.InvariantCulture);
                settings.PlayerStartPositionY = double.Parse(PlayerStartPositionYTextBox.Text, CultureInfo.InvariantCulture);
                settings.TrapWidth = double.Parse(TrapWidthTextBox.Text, CultureInfo.InvariantCulture);
                settings.TrapHeight = double.Parse(TrapHeightTextBox.Text, CultureInfo.InvariantCulture);
                settings.TrapStrokeThickness = double.Parse(TrapStrokeThicknessTextBox.Text, CultureInfo.InvariantCulture);
                settings.EnemyWidth = double.Parse(EnemyWidthTextBox.Text, CultureInfo.InvariantCulture);
                settings.EnemyHeight = double.Parse(EnemyHeightTextBox.Text, CultureInfo.InvariantCulture);
                settings.EnemyMoveSpeed = double.Parse(EnemyMoveSpeedTextBox.Text, CultureInfo.InvariantCulture);
                settings.EnemyRandomDeviation = double.Parse(EnemyRandomDeviationTextBox.Text, CultureInfo.InvariantCulture);
                settings.WindowWidth = double.Parse(WindowWidthTextBox.Text, CultureInfo.InvariantCulture);
                settings.WindowHeight = double.Parse(WindowHeightTextBox.Text, CultureInfo.InvariantCulture);
                settings.PlayerWidth = double.Parse(PlayerWidthTextBox.Text, CultureInfo.InvariantCulture);
                settings.PlayerHeight = double.Parse(PlayerHeightTextBox.Text, CultureInfo.InvariantCulture);
                settings.MusicVolume = double.Parse(MusicVolumeTextBox.Text, CultureInfo.InvariantCulture);

                settings.SaveSettings();

                MessageBox.Show("Настройки успешно сохранены!");

                string appPath = Process.GetCurrentProcess().MainModule.FileName;
                Process.Start(appPath);
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек: {ex.Message}");
            }
        }

        private void MusicVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MusicVolumeTextBox != null)
            {
                MusicVolumeTextBox.Text = e.NewValue.ToString("0.00", CultureInfo.InvariantCulture);
            }
        }

        private void ResetSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = Settings.Instance;

                settings.ResetToDefault();

                LoadSettings();

                MessageBox.Show("Настройки успешно сброшены к значениям по умолчанию!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сбросе настроек: {ex.Message}");
            }
        }
    }
}