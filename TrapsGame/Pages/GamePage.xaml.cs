using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TrapsGame.Processes;
using TrapsGame.Resources;
using TrapsGame.Units;
using TrapsGame.Windows;

namespace TrapsGame.Pages;

public partial class GamePage : Page
{
    #region Fields and Properties

    private readonly GameLogic _gameLogic; 
    private readonly MediaPlayer _soundPlayer = new MediaPlayer(); 

    private bool _isWPressed, _isAPressed, _isSPressed, _isDPressed; // Флаги для отслеживания состояния клавиш
    private DateTime _lastFrameTime = DateTime.Now; // Время последнего кадра

    private MainWindow _mainWindow;
    private MenuPage _menuPage; 

    #endregion

    #region Constructor

    public GamePage(MainWindow mainWindow, MenuPage menuPage)
    {
        InitializeComponent();

        _mainWindow = mainWindow;
        _menuPage = menuPage;

        try
        {
            player.Source = ResDict.GetImage("Player");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
        }

        var playerInstance = new Player(player, MainCanvas, Settings.Instance.PlayerStartPositionX, Settings.Instance.PlayerStartPositionY, Settings.Instance.PlayerMoveStep);
        player.Width = Settings.Instance.PlayerWidth;
        player.Height = Settings.Instance.PlayerHeight;

        _gameLogic = new GameLogic(playerInstance, MainCanvas);

        this.KeyDown += GamePage_KeyDown;
        this.KeyUp += GamePage_KeyUp;

        this.Focusable = true;
        this.Loaded += (s, e) => this.Focus();

        CompositionTarget.Rendering += CompositionTarget_Rendering;

        _gameLogic.GameEnded += OnGameEnded;
        _gameLogic.TrapTriggered += OnTrapTriggered;
        _gameLogic.PlayerDied += OnPlayerDied;

        UpdateTrapCounter();
        UpdateTimeCounter();
        UpdateScoreCounter();
    }

    #endregion

    #region Event Handlers

    private void GamePage_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _isWPressed = true;
                break;
            case Key.A:
                _isAPressed = true;
                break;
            case Key.S:
                _isSPressed = true;
                break;
            case Key.D:
                _isDPressed = true;
                break;
            case Key.Space:
                _gameLogic.PlaceTrap();
                UpdateTrapCounter();
                break;
            case Key.Escape:
                _gameLogic.TogglePause();
                TogglePause();
                break;
        }
    }

    private void GamePage_KeyUp(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _isWPressed = false;
                break;
            case Key.A:
                _isAPressed = false;
                break;
            case Key.S:
                _isSPressed = false;
                break;
            case Key.D:
                _isDPressed = false;
                break;
        }
    }

    private void CompositionTarget_Rendering(object sender, EventArgs e)
    {
        var currentTime = DateTime.Now;
        var elapsedTime = currentTime - _lastFrameTime;

        _gameLogic.Update(_isWPressed, _isAPressed, _isSPressed, _isDPressed, elapsedTime);

        UpdateTimeCounter();
        UpdateScoreCounter();

        _lastFrameTime = currentTime;
    }

    private void OnGameEnded(bool isVictory)
    {
        _mainWindow.ChangePage(new EndGamePage(isVictory, _gameLogic.Score, _gameLogic.ElapsedTime, _mainWindow, _menuPage));
    }

    private void OnTrapTriggered()
    {
        UpdateTrapCounter();
        PlaySound("Resources/boom.mp3");
    }

    private void OnPlayerDied()
    {
        PlaySound("Resources/death.mp3");
        _gameLogic.TogglePause();
        TogglePause();
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        var request = new TraversalRequest(FocusNavigationDirection.Previous);
        if (PauseButton.Focusable)
        {
            PauseButton.MoveFocus(request);
        }

        _gameLogic.TogglePause();
        TogglePause();
    }

    #endregion

    #region Private Methods

    private void PlaySound(string soundPath)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string musicPath = System.IO.Path.Combine(basePath, soundPath);

        Uri soundUri = new Uri(musicPath, UriKind.Relative);
        _soundPlayer.Volume = Settings.Instance.MusicVolume;
        _soundPlayer.Open(soundUri);
        _soundPlayer.Play();
    }

    private void UpdateTrapCounter()
    {
        TrapCounterTextBlock.Text = $"Ловушки: {_gameLogic.AvailableTraps}";
    }

    private void UpdateTimeCounter()
    {
        TimeCounterTextBlock.Text = $"Время: {_gameLogic.ElapsedTime.Minutes:00}:{_gameLogic.ElapsedTime.Seconds:00}";
    }

    private void UpdateScoreCounter()
    {
        ScoreCounterTextBlock.Text = $"Очки: {_gameLogic.Score}";
    }

    private void TogglePause()
    {
        PausePanel.Visibility = _gameLogic.IsPaused ? Visibility.Visible : Visibility.Collapsed;
    }

    #endregion
}