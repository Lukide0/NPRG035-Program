using task_tracker.Task;
using Spectre.Console;

namespace task_tracker.Presenter;

/// <summary> Presenter for removing timers through the UI. </summary>
public class UITimerRemovePresenter : IPresenter
{
    public void Run()
    {
        var timers = App.TimerModel.Find(new TimerFilterOptions());
        var pairs = Model.TimerTaskModel.Pair(timers, App.TaskModel);

        View.TimerView.DisplayTable(pairs);
        Model.Prompt.PressEnter();

        AnsiConsole.Clear();

        var taskID = Model.Prompt.OptionalUInt("Task id");
        if (taskID is null)
        {
            return;
        }

        var timer = App.TimerModel.FindByTaskId((uint)taskID);
        if (timer is null)
        {
            Logger.Err("Timer does not exist");
            Model.Prompt.PressEnter();
            return;
        }

        if (!App.TimerModel.TryRemoveTimer(timer.Id))
        {
            Logger.Err("Cannot remove timer");
            Model.Prompt.PressEnter();
        }
    }
}
