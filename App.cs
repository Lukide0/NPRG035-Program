using System;
using SQLite;
using System.IO;
using task_tracker.Cli;
using task_tracker.Model;
using task_tracker.Presenter;

namespace task_tracker;

/// <summary> Application </summary>
class App
{
    /// <summary> Singleton instance. </summary>
    private static App? _Instance = null;

    /// <summary> Use colors for CLI.</summary>
    public static bool UseColors { get; private set; }

    /// <summary> Verbose mode for CLI.</summary>
    public static bool Verbose { get; private set; }

    /// <summary> Config. </summary>
    public static Config? Config { get; private set; }

    /// <summary> Database filename. </summary>
    public static readonly string DatabaseFileName = "database.db";

    /// <summary> Database connection. </summary>
    public static SQLiteConnection? DBConnection { get; private set; }

    /// <summary> Task model. </summary>
    public static ITaskModel? TaskModel { get; private set; }

    /// <summary> Timer model. </summary>
    public static ITimerModel? TimerModel { get; private set; }

    /// <summary> Get singleton instance. </summary>
    /// <returns> Singleton instance.</returns>
    public static App GetInstance()
    {
        if (_Instance is null)
        {
            _Instance = new App();
        }
        return _Instance;
    }

    public App()
    {
        UseColors = true;
        Verbose = false;
    }

    /// <summary> Runs the application. </summary>
    /// <param name="options"> Options.</param>
    public void Run(CliOptions options)
    {
        Config = Config.TryLoad();
        if (Config is null)
        {
            return;
        }

        try
        {
            Directory.CreateDirectory(Config.StorageDir);
        }
        catch (Exception e)
        {
            Logger.Err($"Cannot create storage directory: {e.Message}");
            return;
        }

        var filepath = Path.Combine(App.Config.StorageDir, App.DatabaseFileName);

        try
        {
            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            }

            DBConnection = new SQLiteConnection(filepath);
            DBConnection.CreateTables<Task.TaskRecord, Task.TimerRecord>();
        }
        catch (SQLiteException ex)
        {
            Logger.Err($"Cannot connect to '{DatabaseFileName}': {ex.Message}");
            return;
        }
        catch (Exception ex)
        {
            Logger.Err($"Cannot create database file: {ex.Message}");
            return;
        }

        TaskModel = new TaskDBModel(DBConnection);
        TimerModel = new TimerDBModel(DBConnection);

        Logger.Info("running");
        UseColors = !options.NoColor;
        Verbose = options.Verbose;

        IPresenter presenter;
        if (options.Options is null)
        {

            presenter = new MenuPresenter();
        }
        else
        {
            presenter = new CliPresenter(options.Options);
        }

        presenter.Run();
        Shutdown();
    }

    private void Shutdown()
    {
        DBConnection.Close();
        Logger.Info("shutting down");
    }
}
