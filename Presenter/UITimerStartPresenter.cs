using Spectre.Console;
using task_tracker.Task;

namespace task_tracker.Presenter;

/// <summary> Presenter for starting timers through the UI. </summary>
public class UITimerStartPresenter : IPresenter
{
    public void Run()
    {
        AnsiConsole.Write(new Rule("Task filter"));

        var filter = Model.Prompt.TaskFilter();

        var tasks = App.TaskModel.Find(filter);

        View.TaskView.DisplayTable(tasks);
        Model.Prompt.PressEnter();

        AnsiConsole.Clear();

        var taskID = Model.Prompt.OptionalUInt("Task id");
        if (taskID is null)
        {
            return;
        }

        var task = App.TaskModel.FindById((uint)taskID);
        if (task is null)
        {
            Logger.Err("Task does not exists");
            Model.Prompt.PressEnter();
            return;
        }

        var timer = App.TimerModel.FindByTaskId((uint)taskID);
        if (timer is not null && timer.State == TimerState.Running)
        {
            Logger.Err("Timer is already running");
            Model.Prompt.PressEnter();
            return;
        }

        if (timer is null)
        {
            timer = new TimerRecord()
            {
                TaskId = (uint)taskID,
                Start = System.DateTime.Now
            };
            if (!App.TimerModel.TryAddTimer(timer))
            {
                Logger.Err("Cannot create new timer");
                Model.Prompt.PressEnter();
            }
        }
        else
        {
            timer.State = TimerState.Running;
            timer.Start = System.DateTime.Now;

            if (!App.TimerModel.TryUpdateTimer(timer))
            {
                Logger.Err("Cannot update timer");
                Model.Prompt.PressEnter();
            }
        }
    }
}
