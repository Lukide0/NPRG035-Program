using System.Collections.Generic;
using Spectre.Console;
using task_tracker.Model;

namespace task_tracker.View;

/// <summary> Timer view. </summary>
public static class TimerView
{
    /// <summary> Display timers. </summary>
    /// <param name="records"> Timer records. </param>
    public static void DisplayTable(IEnumerable<TimerTaskRecord> records)
    {
        var table = new Table();

        table.AddColumns("ID", "Task ID", "Task name", "Timer state", "Total time");

        foreach (var record in records)
        {

            var totalTime = record.Timer.Accumulated;

            if (record.Timer.Start is not null &&
                record.Timer.State == Task.TimerState.Running)
            {
                totalTime +=
                    (System.TimeSpan)(System.DateTime.Now - record.Timer.Start);
            }

            var total = $"{totalTime.Hours}:{totalTime.Minutes}";

            table.AddRow(record.Timer.Id.ToString(), record.Task.Id.ToString(),
                         record.Task.Name, record.Timer.State.ToString(), total);
        }

        AnsiConsole.Write(table);
    }
}
