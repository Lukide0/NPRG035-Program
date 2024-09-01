using Spectre.Console;

namespace task_tracker;

/// <summary >CLI logger.</summary>
public class Logger
{
    /// <summary> Log debug message.</summary>
    /// <param name="message"> Message.</param>
    public static void Debug(string message)
    {
        Log($"[DEBUG]: {message}", color: Color.Green);
    }

    /// <summary> Log info message.</summary>
    /// <param name="message"> Message.</param>
    public static void Info(string message)
    {
        Log($"[INFO]: {message}", color: Color.Cyan1);
    }

    /// <summary> Log warning message.</summary>
    /// <param name="message"> Message.</param>
    public static void Warn(string message)
    {
        Log($"[WARN]: {message}", true, color: Color.Yellow);
    }

    /// <summary> Log error message.</summary>
    /// <param name="message"> Message.</param>
    public static void Err(string message)
    {
        Log($"[ERR]: {message}", true, color: Color.Red);
    }

    /// <summary> Log data.</summary>
    /// <param name="obj"> Object.</param>
    public static void Data<T>(T obj) { AnsiConsole.WriteLine(obj.ToString()); }

    /// <summary> Log message.</summary>
    /// <param name="message"> Message.</param>
    /// <param name="force"> Force log.</param>
    /// <param name="color"> Color.</param>
    private static void Log(string message, bool force = false,
                            Color? color = null)

    {
        if (App.Verbose || force)
        {
            if (!App.UseColors)
            {
                color = null;
            }

            var txt = new Text(message, new Style(color ?? Color.Default));

            AnsiConsole.Write(txt);
            AnsiConsole.WriteLine();
        }
    }
}
