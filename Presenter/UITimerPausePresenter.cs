using Spectre.Console;
using task_tracker.Task;

namespace task_tracker.Presenter;

/// <summary> Presenter for pausing timers through the UI. </summary>
public class UITimerPausePresenter : IPresenter
{
    public void Run()
    {

        var timers = App.TimerModel.FindRunning();
        var pairs = Model.TimerTaskModel.Pair(timers, App.TaskModel);

        View.TimerView.DisplayTable(pairs);
        Model.Prompt.PressEnter();

        AnsiConsole.Clear();

        var taskIdOpt = Model.Prompt.OptionalUInt("Task id");

        if (taskIdOpt is null)
        {
            return;
        }

        uint taskID = (uint)taskIdOpt;

        var task = App.TaskModel.FindById(taskID);
        if (task is null)
        {
            Logger.Err("Task does not exists");
            Model.Prompt.PressEnter();
            return;
        }

        var timer = App.TimerModel.FindByTaskId(taskID);
        if (timer is null || timer.State == TimerState.Paused)
        {
            Logger.Err("Timer is paused");
            Model.Prompt.PressEnter();
            return;
        }

        System.Diagnostics.Debug.Assert(timer.Start is not null);

        timer.State = TimerState.Paused;
        timer.Accumulated += (System.TimeSpan)(System.DateTime.Now - timer.Start);

        if (!App.TimerModel.TryUpdateTimer(timer))
        {
            Logger.Err("Cannot update timer");
            Model.Prompt.PressEnter();
        }
    }
}
