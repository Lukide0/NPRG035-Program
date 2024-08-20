using System;
using System.Diagnostics;
using task_tracker.Cli;

namespace task_tracker.Presenter;

/// <summary> Presenter for filtering tasks through the CLI. </summary>
public class CliTaskFilterPresenter : IPresenter
{
    private FilterTaskOptions _Opts;

    public CliTaskFilterPresenter(FilterTaskOptions opts) { _Opts = opts; }

    public void Run()
    {
        Debug.Assert(App.TaskModel is not null);
        var records = App.TaskModel.Find(_Opts.Filter);

        Console.WriteLine("id;name;description;state;priority;deadline");
        foreach (var rec in records)
        {
            // id, name, description, priority, state, deadline
            Console.WriteLine(
                $"{rec.Id};{rec.Name};{rec.Description};{rec.State};{rec.Priority};{rec.Deadline?.ToString("dd.MM.yyyy HH:mm")}");
        }
    }
}
