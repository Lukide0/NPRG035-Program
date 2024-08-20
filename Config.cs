using System;
using System.Text.Json;
using System.IO;

namespace task_tracker;

public class Config
{
    private static string _CONFIG_NAME = "config.json";
    private static string _CONFIG_PATH =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _CONFIG_NAME);

    public string StorageDir { get; private set; }
    public string Locale { get; private set; } = "cs";

    public Config()
    {
        StorageDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage");
    }

    /// <summary> Tries to load config. </summary>
    /// <returns> Config.</returns>
    public static Config? TryLoad()
    {
        Logger.Info("Loading config");

        if (File.Exists(_CONFIG_PATH))
        {
            return Load();
        }

        Logger.Warn("Config does not exits");
        Logger.Info("Creating new config.json");

        var config = new Config();

        string jsonStr = JsonSerializer.Serialize(config);

        try
        {
            File.WriteAllText(_CONFIG_PATH, jsonStr, System.Text.Encoding.UTF8);
        }
        catch (Exception e)
        {
            Logger.Err($"Cannot create config: {e.Message}");
            return null;
        }

        Logger.Info($"Config created: {_CONFIG_PATH}");
        return config;
    }

    /// <summary> Tries to load config. </summary>
    /// <returns> Config.</returns>
    private static Config? Load()
    {
        string content;
        try
        {
            content = File.ReadAllText(_CONFIG_PATH);
        }
        catch (Exception e)
        {
            Logger.Err($"Cannot read config: {e.Message}");
            return null;
        }

        try
        {
            Config config = JsonSerializer.Deserialize<Config>(content);
            return config;
        }
        catch (Exception e)
        {
            Logger.Err($"Cannot deserialize config: {e.Message}");
            return null;
        }
    }
}
