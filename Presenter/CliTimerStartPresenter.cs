using System.Diagnostics;
using task_tracker.Task;
using task_tracker.Cli;

namespace task_tracker.Presenter;

/// <summary> Presenter for starting timers through the CLI. </summary>
public class CliTimerStartPresenter : IPresenter
{
    private StartTimerOptions _Opts;

    public CliTimerStartPresenter(StartTimerOptions opts) { _Opts = opts; }

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
        else if (taskRecord.State == Task.TaskState.Done)
        {
            Logger.Err("Cannot start timer on completed task");
            return;
        }

        Logger.Info("Searching for timer");
        var timerRecord = App.TimerModel.FindByTaskId(taskId);

        if (timerRecord is null)
        {
            CreateNewTimer(taskId);
        }
        else if (timerRecord.State != TimerState.Running)
        {
            StartTimer(timerRecord);
        }
        else
        {
            Logger.Err("Cannot start already running timer");
        }
    }

    private void CreateNewTimer(uint taskId)
    {
        Logger.Info("Creating new timer");

        var timer =
            new TimerRecord() { TaskId = taskId, Start = System.DateTime.Now };

        if (!App.TimerModel.TryAddTimer(timer))
        {
            Logger.Err("Cannot create new timer");
            return;
        }

        Logger.Info($"Created timer with id '{timer.Id}'");
    }

    private void StartTimer(TimerRecord record)
    {
        Logger.Info("Starting timer");

        record.State = TimerState.Running;
        record.Start = System.DateTime.Now;

        if (!App.TimerModel.TryUpdateTimer(record))
        {
            Logger.Err("Cannot update timer");
            return;
        }
        Logger.Info("Timer resumed");
    }
}
