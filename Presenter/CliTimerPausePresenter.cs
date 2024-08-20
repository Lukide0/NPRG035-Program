using System.Diagnostics;
using task_tracker.Cli;
using task_tracker.Task;
using System;

namespace task_tracker.Presenter;

/// <summary> Presenter for pausing timers through the CLI. </summary>
public class CliTimerPausePresenter : IPresenter
{
    private PauseTimerOptions _Opts;

    public CliTimerPausePresenter(PauseTimerOptions opts) { _Opts = opts; }

    public void Run()
    {
        Debug.Assert(App.TimerModel is not null && App.TaskModel is not null);

        var taskId = _Opts.taskId;

        Logger.Info("Searching for task");
        var taskRecord = App.TaskModel.FindById(taskId);

        if (taskRecord is null)
        {
            Logger.Err("Task does not exists");
            return;
        }

        Logger.Info("Searching for timer");
        var timerRecord = App.TimerModel.FindByTaskId(taskId);

        if (timerRecord is null)
        {
            Logger.Err("Timer does not exists");
            return;
        }
        else if (timerRecord.State != TimerState.Running)
        {
            Logger.Err("Timer is already stopped");
            return;
        }

        Debug.Assert(timerRecord.Start is not null);

        timerRecord.State = TimerState.Paused;
        timerRecord.Accumulated += (DateTime.Now - (DateTime)timerRecord.Start);
        timerRecord.Start = null;

        if (!App.TimerModel.TryUpdateTimer(timerRecord))
        {
            Logger.Err("Cannot stop timer");
        }
        else
        {
            Logger.Info("Timer stopped");
        }
    }
}
