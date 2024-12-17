using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
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
    private readonly List<Enemy> _enemies = new(); // Список врагов
    private int _availableTraps = 10; // Доступное количество ловушек

    private readonly DispatcherTimer _enemySpawnTimer; // Таймер для создания врагов
    private readonly DispatcherTimer _difficultyTimer; // Таймер для увеличения сложности
    private readonly Random _random = new(); // Для случайного появления врагов

    private TimeSpan _currentSpawnInterval = TimeSpan.FromSeconds(2); // Текущий интервал появления врагов
    private const double MinSpawnInterval = 0.1; // Минимальный интервал появления врагов

    private int _score = 0; // Счет игрока
    private DateTime _startTime = DateTime.Now; // Время начала игры
    private readonly DispatcherTimer _scoreTimer; // Таймер для обновления счета
    private const int ScorePerSecond = 1; // Очки за каждую секунду
    private const int ScorePerEnemy = 50; // Очки за уничтоженного врага
    private const int VictoryTime = 60; // Время для победы (в секундах)

    private bool _isPaused = false; // Флаг для отслеживания состояния паузы
    private DateTime _pauseStartTime; // Время начала паузы
    private TimeSpan _totalPauseTime = TimeSpan.Zero; // Общее время паузы

    MainWindow _mainWindow;
    MenuPage _menuPage;

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
        _player = new Player(player, MainCanvas, 350, 350, 250);

        this.KeyDown += GamePage_KeyDown;
        this.KeyUp += GamePage_KeyUp;

        this.Focusable = true;
        this.Loaded += (s, e) => this.Focus();

        CompositionTarget.Rendering += CompositionTarget_Rendering;

        UpdateTrapCounter();
        UpdateTimeCounter();
        UpdateScoreCounter();

        _enemySpawnTimer = new DispatcherTimer
        {
            Interval = _currentSpawnInterval
        };
        _enemySpawnTimer.Tick += EnemySpawnTimer_Tick;
        _enemySpawnTimer.Start();

        _difficultyTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2)
        };
        _difficultyTimer.Tick += DifficultyTimer_Tick;
        _difficultyTimer.Start();

        _scoreTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _scoreTimer.Tick += ScoreTimer_Tick;
        _scoreTimer.Start();
    }

    private void UpdateTimeCounter()
    {
        var elapsedTime = DateTime.Now - _startTime - _totalPauseTime;
        TimeCounterTextBlock.Text = $"Время: {elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}";
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
            case Key.Escape:
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


    private void TogglePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            _enemySpawnTimer.Stop();
            _difficultyTimer.Stop();
            _scoreTimer.Stop();

            _pauseStartTime = DateTime.Now;

            PausePanel.Visibility = Visibility.Visible;
        }
        else
        {
            _enemySpawnTimer.Start();
            _difficultyTimer.Start();
            _scoreTimer.Start();

            _totalPauseTime += DateTime.Now - _pauseStartTime;

            PausePanel.Visibility = Visibility.Collapsed;
        }
    }



    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        TogglePause();
    }

    private void CompositionTarget_Rendering(object sender, EventArgs e)
    {
        if (_isPaused) 
            return;

        var currentTime = DateTime.Now;
        var elapsedTime = currentTime - _lastFrameTime;

        _player.Move(_isWPressed, _isAPressed, _isSPressed, _isDPressed, elapsedTime);

        double playerX = Canvas.GetLeft(player);
        double playerY = Canvas.GetTop(player);

        foreach (var enemy in _enemies)
        {
            enemy.UpdatePosition(playerX, playerY);
        }

        CheckCollisions();

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

        double trapX = playerX + player.ActualWidth / 2;
        double trapY = playerY + player.ActualHeight / 2;

        var trap = new Trap(MainCanvas, trapX, trapY);

        _traps.Add(trap);

        _trapPositions.Add(new Point(trapX, trapY));

        _availableTraps--;

        UpdateTrapCounter();

    }


    private void ScoreTimer_Tick(object sender, EventArgs e)
    {
        if (_isPaused) 
            return;

        _score += ScorePerSecond;

        UpdateScoreCounter();

        UpdateTimeCounter();

        var elapsedTime = DateTime.Now - _startTime - _totalPauseTime;
        if (elapsedTime.TotalSeconds >= VictoryTime)
        {
            _mainWindow.ChangePage(new EndGamePage(true, _score, DateTime.Now - _startTime - _totalPauseTime, _mainWindow, _menuPage));
            TogglePause();
        }
    } 


    private void UpdateScoreCounter()
    {
        ScoreCounterTextBlock.Text = $"Очки: {_score}";
    }

  

    private async void RemoveTrapAfterDelay(Trap trap, TimeSpan delay)
    {
        await Task.Delay(delay);
        trap.Remove();
        _traps.Remove(trap);

        _availableTraps++;

        UpdateTrapCounter();
    }

    private void UpdateTrapCounter()
    {
        TrapCounterTextBlock.Text = $"Ловушки: {_availableTraps}";
    }

    private void EnemySpawnTimer_Tick(object sender, EventArgs e)
    {
        double enemyX, enemyY;
        double playerX = Canvas.GetLeft(player);
        double playerY = Canvas.GetTop(player);

        do
        {
            enemyX = _random.NextDouble() * (MainCanvas.ActualWidth - 30);
            enemyY = _random.NextDouble() * (MainCanvas.ActualHeight - 30);
        }
        while (Math.Sqrt(Math.Pow(enemyX - playerX, 2) + Math.Pow(enemyY - playerY, 2)) < 200);

        var enemy = new Enemy(MainCanvas, enemyX, enemyY);
        _enemies.Add(enemy);
    }

    private void DifficultyTimer_Tick(object sender, EventArgs e)
    {
        if (_currentSpawnInterval.TotalSeconds > MinSpawnInterval)
        {
            _currentSpawnInterval = TimeSpan.FromSeconds(_currentSpawnInterval.TotalSeconds * 0.9);
            _enemySpawnTimer.Interval = _currentSpawnInterval;
        }
    }

    private void CheckCollisions()
    {
        foreach (var enemy in _enemies.ToList())
        {
            foreach (var trap in _traps.ToList())
            {
                if (IsColliding(enemy, trap))
                {
                    enemy.Remove();
                    _enemies.Remove(enemy);

                    _traps.Remove(trap);
                    trap.Remove();
                    _availableTraps++;
                    UpdateTrapCounter();

                    _score += ScorePerEnemy;
                    UpdateScoreCounter();
                    break;
                }
            }

            if (IsColliding(enemy, player))
            {
                _mainWindow.ChangePage(new EndGamePage(false, _score, DateTime.Now - _startTime - _totalPauseTime, _mainWindow, _menuPage));
                TogglePause();
            }
        }
    }


    private bool IsColliding(Enemy enemy, Trap trap)
    {
        return enemy.X < trap.X + trap.Width &&
               enemy.X + enemy.Width > trap.X &&
               enemy.Y < trap.Y + trap.Height &&
               enemy.Y + enemy.Height > trap.Y;
    }

    private bool IsColliding(Enemy enemy, Image player)
    {
        double playerX = Canvas.GetLeft(player);
        double playerY = Canvas.GetTop(player);

        return enemy.X < playerX + player.ActualWidth &&
               enemy.X + enemy.Width > playerX &&
               enemy.Y < playerY + player.ActualHeight &&
               enemy.Y + enemy.Height > playerY;
    }
}