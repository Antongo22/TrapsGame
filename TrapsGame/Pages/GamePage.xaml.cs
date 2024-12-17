using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TrapsGame.Resources;
using TrapsGame.Units;
using TrapsGame.Windows;

namespace TrapsGame.Pages;

public partial class GamePage : Page
{
    private bool _isWPressed, _isAPressed, _isSPressed, _isDPressed; // Флаги для отслеживания состояния клавиш

    private DateTime _lastFrameTime = DateTime.Now; // Время последнего кадра

    private readonly Player _player; // Экземпляр класса Player
    private readonly List<Trap> _traps = new(); // Список ловушек
    private readonly List<Point> _trapPositions = new(); // Список координат ловушек
    private int _availableTraps = 10; // Доступное количество ловушек

    public GamePage(MainWindow mainWindow, MenuPage menuPage)
    {
        InitializeComponent();

        try
        {
            player.Source = ResDict.GetImage("Player");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
        }
        _player = new Player(player, MainCanvas, 50, 50, 150);

        this.KeyDown += GamePage_KeyDown;
        this.KeyUp += GamePage_KeyUp;

        this.Focusable = true;
        this.Loaded += (s, e) => this.Focus();

        CompositionTarget.Rendering += CompositionTarget_Rendering;

        UpdateTrapCounter();
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
            case Key.Space: 
                PlaceTrap();
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

    private void PlaceTrap()
    {
        if (_availableTraps <= 0)
        {
            return;
        }

        double playerX = Canvas.GetLeft(player);
        double playerY = Canvas.GetTop(player);

        // Рассчитываем центр игрока
        double trapX = playerX + player.ActualWidth / 2 ; 
        double trapY = playerY + player.ActualHeight / 2 ; 

        var trap = new Trap(MainCanvas, trapX, trapY);

        _traps.Add(trap);

        _trapPositions.Add(new Point(trapX, trapY));

        _availableTraps--;

        UpdateTrapCounter();

        RemoveTrapAfterDelay(trap, TimeSpan.FromSeconds(5));
    }

    private async void RemoveTrapAfterDelay(Trap trap, TimeSpan delay)
    {
        await Task.Delay(delay); 
        trap.Remove(MainCanvas); 
        _traps.Remove(trap); 

        _availableTraps++;

        UpdateTrapCounter();
    }

    private void UpdateTrapCounter()
    {
        TrapCounterTextBlock.Text = $"Ловушки: {_availableTraps}";
    }
}