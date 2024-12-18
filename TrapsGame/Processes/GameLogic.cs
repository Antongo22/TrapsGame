using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TrapsGame.Processes;
using TrapsGame.Units;

namespace TrapsGame.Processes;

public class GameLogic
{
    #region Fields and Properties

    private readonly Player _player; // Игрок
    private readonly List<Trap> _traps = new(); // Список ловушек
    private readonly List<Enemy> _enemies = new(); // Список врагов
    private readonly List<Point> _trapPositions = new(); // Список координат ловушек
    private readonly Canvas _gameCanvas; // Игровое поле
    private readonly Random _random = new(); // Для случайного появления врагов

    private int _availableTraps; // Доступное количество ловушек
    private int _score = 0; // Счет игрока
    private DateTime _startTime = DateTime.Now; // Время начала игры
    private TimeSpan _totalPauseTime = TimeSpan.Zero; // Общее время паузы
    private bool _isPaused = false; // Флаг для отслеживания состояния паузы

    public readonly DispatcherTimer _enemySpawnTimer; // Таймер для создания врагов
    private readonly DispatcherTimer _difficultyTimer; // Таймер для увеличения сложности
    private readonly DispatcherTimer _scoreTimer; // Таймер для обновления счета

    // События
    public event Action<bool> GameEnded; // Событие завершения игры (победа/поражение)
    public event Action TrapTriggered; // Событие срабатывания ловушки
    public event Action PlayerDied; // Событие смерти игрока

    // Свойства для доступа к данным
    public int Score => _score; // Текущий счет игрока
    public int AvailableTraps => _availableTraps; // Доступное количество ловушек
    public TimeSpan ElapsedTime => DateTime.Now - _startTime - _totalPauseTime; // Прошедшее время игры
    public bool IsPaused => _isPaused; // Флаг паузы

    #endregion

    #region Constructor

    public GameLogic(Player player, Canvas gameCanvas)
    {
        _player = player;
        _gameCanvas = gameCanvas;

        _availableTraps = Settings.Instance.InitialAvailableTraps;

        _enemySpawnTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(Settings.Instance.InitialSpawnInterval)
        };
        _enemySpawnTimer.Tick += EnemySpawnTimer_Tick;
        _enemySpawnTimer.Start();

        _difficultyTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(Settings.Instance.TimeIntervalForTheAppearanceOfEnemies)
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

    #endregion

    #region Public Methods

    /// <summary>
    /// Обновляет состояние игры.
    /// </summary>
    /// <param name="isWPressed">Нажата ли клавиша W</param>
    /// <param name="isAPressed">Нажата ли клавиша A</param>
    /// <param name="isSPressed">Нажата ли клавиша S</param>
    /// <param name="isDPressed">Нажата ли клавиша D</param>
    /// <param name="elapsedTime">Прошедшее время с последнего кадра</param>
    public void Update(bool isWPressed, bool isAPressed, bool isSPressed, bool isDPressed, TimeSpan elapsedTime)
    {
        if (_isPaused)
            return;

        _player.Move(isWPressed, isAPressed, isSPressed, isDPressed, elapsedTime);

        foreach (var enemy in _enemies)
        {
            enemy.UpdatePosition(_player.X, _player.Y);
        }

        CheckCollisions();
    }

    /// <summary>
    /// Размещает ловушку в позиции игрока.
    /// </summary>
    public void PlaceTrap()
    {
        if (_availableTraps <= 0)
            return;

        double trapX = _player.X + _player.Width / 2;
        double trapY = _player.Y + _player.Height / 2;

        var trap = new Trap(_gameCanvas, trapX, trapY);
        _traps.Add(trap);
        _trapPositions.Add(new Point(trapX, trapY));

        _availableTraps--;
    }

    /// <summary>
    /// Переключает состояние паузы.
    /// </summary>
    public void TogglePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            _enemySpawnTimer.Stop();
            _difficultyTimer.Stop();
            _scoreTimer.Stop();
        }
        else
        {
            _enemySpawnTimer.Start();
            _difficultyTimer.Start();
            _scoreTimer.Start();
        }
    }

    #endregion

    #region Private Methods

    private void EnemySpawnTimer_Tick(object sender, EventArgs e)
    {
        double enemyX, enemyY;

        do
        {
            enemyX = _random.NextDouble() * (_gameCanvas.ActualWidth - 30);
            enemyY = _random.NextDouble() * (_gameCanvas.ActualHeight - 30);
        }
        while (Math.Sqrt(Math.Pow(enemyX - _player.X, 2) + Math.Pow(enemyY - _player.Y, 2)) < Settings.Instance.EnemySpawnDistance);

        var enemy = new Enemy(_gameCanvas, enemyX, enemyY);
        _enemies.Add(enemy);
    }

    public void DifficultyTimer_Tick(object sender, EventArgs e)
    {
        if (_enemySpawnTimer.Interval.TotalSeconds > Settings.Instance.MinSpawnInterval)
        {
            _enemySpawnTimer.Interval = TimeSpan.FromSeconds(_enemySpawnTimer.Interval.TotalSeconds * Settings.Instance.CoefficientOfIntervalReduction);
        }
    }

    private void ScoreTimer_Tick(object sender, EventArgs e)
    {
        if (_isPaused)
            return;

        _score += Settings.Instance.ScorePerSecond;

        var elapsedTime = DateTime.Now - _startTime - _totalPauseTime;
        if (elapsedTime.TotalSeconds >= Settings.Instance.VictoryTime)
        {
            OnGameEnded(true);
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

                    _score += Settings.Instance.ScorePerEnemy;

                    OnTrapTriggered();

                    break;
                }
            }

            if (IsColliding(enemy, _player))
            {
                OnPlayerDied();
                OnGameEnded(false);
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

    private bool IsColliding(Enemy enemy, Player player)
    {
        return enemy.X < player.X + player.Width &&
               enemy.X + enemy.Width > player.X &&
               enemy.Y < player.Y + player.Height &&
               enemy.Y + enemy.Height > player.Y;
    }

    public void OnGameEnded(bool isVictory)
    {
        GameEnded?.Invoke(isVictory);

        _isPaused = true;
        _enemySpawnTimer.Stop();
        _difficultyTimer.Stop();
        _scoreTimer.Stop();
    }

    private void OnTrapTriggered()
    {
        TrapTriggered?.Invoke();
    }

    private void OnPlayerDied()
    {
        PlayerDied?.Invoke();
    }

    #endregion
}