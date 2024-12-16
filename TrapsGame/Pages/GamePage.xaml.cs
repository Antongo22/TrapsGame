using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TrapsGame.Pages;
using TrapsGame.Units;
using TrapsGame.Windows;

namespace TrapsGame.Pages;

public partial class GamePage : Page
{
    private bool _isWPressed, _isAPressed, _isSPressed, _isDPressed; // Флаги для отслеживания состояния клавиш

    private DateTime _lastFrameTime = DateTime.Now; 

    private readonly Player _player; 

    public GamePage(MainWindow mainWindow, MenuPage menuPage)
    {
        InitializeComponent();

        _player = new Player(player, MainCanvas, 50, 50, 150);

        this.KeyDown += GamePage_KeyDown;
        this.KeyUp += GamePage_KeyUp;

        this.Focusable = true;
        this.Loaded += (s, e) => this.Focus();

        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

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

        _player.Move(_isWPressed, _isAPressed, _isSPressed, _isDPressed, elapsedTime);

        _lastFrameTime = currentTime;
    }
}