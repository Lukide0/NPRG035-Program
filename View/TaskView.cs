using Spectre.Console;
using task_tracker.Task;
using System.Collections;
using System.Collections.Generic;

namespace task_tracker.View;

/// <summary> Task view. </summary>
public static class TaskView
{

    /// <summary> Display task. </summary>
    /// <param name="record"> Task record. </param>
    public static void Display(TaskRecord record)
    {

        var grid = new Grid();

        grid.AddColumns(2);

        grid.AddRow(new string[] { "[bold]Name:[/]", record.Name });
        grid.AddRow(new string[] { "[bold]Description:[/]", record.Description });
        grid.AddRow(
            new string[] { "[bold]Priority:[/]", record.Priority.ToString() });
        grid.AddRow(new string[] { "[bold]State:[/]", record.State.ToString() });

        var deadlineStr = record.Deadline?.ToString("dd.MM.yyyy HH:mm") ?? "";
        grid.AddRow(new string[] { "[bold]Deadline:[/]", deadlineStr });

        AnsiConsole.Write(grid);
    }

    /// <summary> Display tasks. </summary>
    /// <param name="records"> Task records. </param>
    public static void DisplayTable(IEnumerable<TaskRecord> records)
    {
        var table = new Table();
        table.AddColumns("ID", "Name", "Description", "Priority", "State",
                         "Deadline");

        foreach (var record in records)
        {
            table.AddRow(record.Id.ToString(), record.Name, record.Description,
                         record.Priority.ToString(), record.State.ToString(),
                         record.Deadline?.ToString("dd.MM.yyyy HH:mm") ?? "");
        }

        AnsiConsole.Write(table);
    }
}
