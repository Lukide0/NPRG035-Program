using System;
using System.Diagnostics;
using task_tracker.Cli;

namespace task_tracker.Presenter;

/// <summary> Presenter for filtering timers through the CLI. </summary>
public class CliTimerFilterPresenter : IPresenter
{
    private FilterTimerOptions _Opts;

    public CliTimerFilterPresenter(FilterTimerOptions opts) { _Opts = opts; }

    public void Run()
    {
        Debug.Assert(App.TimerModel is not null && App.TaskModel is not null);
        var records = App.TimerModel.Find(_Opts.Filter);

        Console.WriteLine("id;task_id;state;accumulated(H:m);start");
        foreach (var rec in records)
        {
            var accumulated = rec.Accumulated.Hours.ToString() + ':' +
                              rec.Accumulated.Minutes.ToString();

            Console.WriteLine(
                $"{rec.Id};{rec.TaskId};{rec.State};{accumulated};{rec.Start?.ToString("dd.MM.yyyy HH:mm")}");
        }
    }
}
