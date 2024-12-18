using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace TrapsGame.Processes;

public class Settings
{
    private const string SettingsFilePath = "settings.xml";

    private static readonly Lazy<Settings> _instance = new(() => new Settings());

    public int InitialAvailableTraps { get; set; }
    public double InitialSpawnInterval { get; set; }
    public double MinSpawnInterval { get; set; }
    public int ScorePerSecond { get; set; }
    public int ScorePerEnemy { get; set; }
    public int VictoryTime { get; set; }
    public double PlayerMoveStep { get; set; }
    public double EnemySpawnDistance { get; set; }
    public double CoefficientOfIntervalReduction { get; set; }
    public double TimeIntervalForTheAppearanceOfEnemies { get; set; }
    public double PlayerStartPositionX { get; set; }
    public double PlayerStartPositionY { get; set; }
    public double TrapWidth { get; set; }
    public double TrapHeight { get; set; }
    public double TrapStrokeThickness { get; set; }
    public double EnemyWidth { get; set; }
    public double EnemyHeight { get; set; }
    public double EnemyMoveSpeed { get; set; }
    public double EnemyRandomDeviation { get; set; }
    public double WindowWidth { get; set; }
    public double WindowHeight { get; set; }
    public double PlayerWidth { get; set; } 
    public double PlayerHeight { get; set; }
    public double MusicVolume { get; set; }

    private Settings()
    {
        LoadSettings();
    }

    public static Settings Instance => _instance.Value;

    private void LoadSettings()
    {
        if (!File.Exists(SettingsFilePath))
        {
            CreateDefaultSettingsFile();
        }

        try
        {
            var xml = XDocument.Load(SettingsFilePath);
            var root = xml.Element("Settings");

            if (root == null)
            {
                throw new InvalidOperationException("Invalid settings file format.");
            }

            InitialAvailableTraps = int.Parse(root.Element("InitialAvailableTraps")?.Value ?? "10");
            InitialSpawnInterval = double.Parse(root.Element("InitialSpawnInterval")?.Value ?? "2.0", CultureInfo.InvariantCulture);
            MinSpawnInterval = double.Parse(root.Element("MinSpawnInterval")?.Value ?? "0.1", CultureInfo.InvariantCulture);
            ScorePerSecond = int.Parse(root.Element("ScorePerSecond")?.Value ?? "1");
            ScorePerEnemy = int.Parse(root.Element("ScorePerEnemy")?.Value ?? "50");
            VictoryTime = int.Parse(root.Element("VictoryTime")?.Value ?? "60");
            PlayerMoveStep = double.Parse(root.Element("PlayerMoveStep")?.Value ?? "250", CultureInfo.InvariantCulture);
            EnemySpawnDistance = double.Parse(root.Element("EnemySpawnDistance")?.Value ?? "200", CultureInfo.InvariantCulture);
            CoefficientOfIntervalReduction = double.Parse(root.Element("CoefficientOfIntervalReduction")?.Value ?? "0.9", CultureInfo.InvariantCulture);
            TimeIntervalForTheAppearanceOfEnemies = double.Parse(root.Element("TimeIntervalForTheAppearanceOfEnemies")?.Value ?? "2", CultureInfo.InvariantCulture);
            PlayerStartPositionX = double.Parse(root.Element("PlayerStartPositionX")?.Value ?? "350", CultureInfo.InvariantCulture);
            PlayerStartPositionY = double.Parse(root.Element("PlayerStartPositionY")?.Value ?? "350", CultureInfo.InvariantCulture);
            TrapWidth = double.Parse(root.Element("TrapWidth")?.Value ?? "30", CultureInfo.InvariantCulture);
            TrapHeight = double.Parse(root.Element("TrapHeight")?.Value ?? "30", CultureInfo.InvariantCulture);
            TrapStrokeThickness = double.Parse(root.Element("TrapStrokeThickness")?.Value ?? "2", CultureInfo.InvariantCulture);
            EnemyWidth = double.Parse(root.Element("EnemyWidth")?.Value ?? "30", CultureInfo.InvariantCulture);
            EnemyHeight = double.Parse(root.Element("EnemyHeight")?.Value ?? "30", CultureInfo.InvariantCulture);
            EnemyMoveSpeed = double.Parse(root.Element("EnemyMoveSpeed")?.Value ?? "1", CultureInfo.InvariantCulture);
            EnemyRandomDeviation = double.Parse(root.Element("EnemyRandomDeviation")?.Value ?? "1.0", CultureInfo.InvariantCulture);
            WindowWidth = double.Parse(root.Element("WindowWidth")?.Value ?? "800", CultureInfo.InvariantCulture);
            WindowHeight = double.Parse(root.Element("WindowHeight")?.Value ?? "600", CultureInfo.InvariantCulture);
            PlayerWidth = double.Parse(root.Element("PlayerWidth")?.Value ?? "80", CultureInfo.InvariantCulture);
            PlayerHeight = double.Parse(root.Element("PlayerHeight")?.Value ?? "80", CultureInfo.InvariantCulture);
            MusicVolume = double.Parse(root.Element("MusicVolume")?.Value ?? "0.5", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load settings.", ex);
        }
    }

    private void CreateDefaultSettingsFile()
    {
        var defaultSettings = new XDocument(
            new XElement("Settings",
                new XElement("InitialAvailableTraps", "10"),
                new XElement("InitialSpawnInterval", "2.0"),
                new XElement("MinSpawnInterval", "0.1"),
                new XElement("ScorePerSecond", "1"),
                new XElement("ScorePerEnemy", "50"),
                new XElement("VictoryTime", "60"),
                new XElement("PlayerMoveStep", "250"),
                new XElement("EnemySpawnDistance", "200"),
                new XElement("CoefficientOfIntervalReduction", "0.9"),
                new XElement("TimeIntervalForTheAppearanceOfEnemies", "2"),
                new XElement("PlayerStartPositionX", "350"),
                new XElement("PlayerStartPositionY", "350"),
                new XElement("TrapWidth", "30"),
                new XElement("TrapHeight", "30"),
                new XElement("TrapStrokeThickness", "0"),
                new XElement("EnemyWidth", "30"),
                new XElement("EnemyHeight", "30"),
                new XElement("EnemyMoveSpeed", "1"),
                new XElement("EnemyRandomDeviation", "1.0"),
                new XElement("WindowWidth", "800"), 
                new XElement("WindowHeight", "800"),
                new XElement("PlayerWidth", "80"),
                new XElement("PlayerHeight", "80"),
                new XElement("MusicVolume", "0.5")
            )
        );

        defaultSettings.Save(SettingsFilePath);
    }

    public void ResetToDefault()
    {
        CreateDefaultSettingsFile();
        LoadSettings();
    }

    public void SaveSettings()
    {
        var settingsXml = new XDocument(
            new XElement("Settings",
                new XElement("InitialAvailableTraps", InitialAvailableTraps),
                new XElement("InitialSpawnInterval", InitialSpawnInterval.ToString(CultureInfo.InvariantCulture)),
                new XElement("MinSpawnInterval", MinSpawnInterval.ToString(CultureInfo.InvariantCulture)),
                new XElement("ScorePerSecond", ScorePerSecond),
                new XElement("ScorePerEnemy", ScorePerEnemy),
                new XElement("VictoryTime", VictoryTime),
                new XElement("PlayerMoveStep", PlayerMoveStep.ToString(CultureInfo.InvariantCulture)),
                new XElement("EnemySpawnDistance", EnemySpawnDistance.ToString(CultureInfo.InvariantCulture)),
                new XElement("CoefficientOfIntervalReduction", CoefficientOfIntervalReduction.ToString(CultureInfo.InvariantCulture)),
                new XElement("TimeIntervalForTheAppearanceOfEnemies", TimeIntervalForTheAppearanceOfEnemies.ToString(CultureInfo.InvariantCulture)),
                new XElement("PlayerStartPositionX", PlayerStartPositionX.ToString(CultureInfo.InvariantCulture)),
                new XElement("PlayerStartPositionY", PlayerStartPositionY.ToString(CultureInfo.InvariantCulture)),
                new XElement("TrapWidth", TrapWidth.ToString(CultureInfo.InvariantCulture)),
                new XElement("TrapHeight", TrapHeight.ToString(CultureInfo.InvariantCulture)),
                new XElement("TrapStrokeThickness", TrapStrokeThickness.ToString(CultureInfo.InvariantCulture)),
                new XElement("EnemyWidth", EnemyWidth.ToString(CultureInfo.InvariantCulture)),
                new XElement("EnemyHeight", EnemyHeight.ToString(CultureInfo.InvariantCulture)),
                new XElement("EnemyMoveSpeed", EnemyMoveSpeed.ToString(CultureInfo.InvariantCulture)),
                new XElement("EnemyRandomDeviation", EnemyRandomDeviation.ToString(CultureInfo.InvariantCulture)),
                new XElement("WindowWidth", WindowWidth.ToString(CultureInfo.InvariantCulture)),
                new XElement("WindowHeight", WindowHeight.ToString(CultureInfo.InvariantCulture)),
                new XElement("PlayerWidth", PlayerWidth.ToString(CultureInfo.InvariantCulture)),
                new XElement("PlayerHeight", PlayerHeight.ToString(CultureInfo.InvariantCulture)),
                new XElement("MusicVolume", MusicVolume.ToString(CultureInfo.InvariantCulture))
                )            
             );

        settingsXml.Save(SettingsFilePath);
    }
}