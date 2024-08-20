using task_tracker.Task;
using Spectre.Console;

namespace task_tracker.Presenter;

/// <summary> Presenter for filtering timers through the UI. </summary>
public class UITimerFilterPresenter : IPresenter
{

    public void Run()
    {
        AnsiConsole.Write(new Rule("Timer filter").LeftJustified());

        var filter = new TimerFilterOptions();
        filter.Id = Model.Prompt.OptionalUInt("Id");
        filter.State = Model.Prompt.OptionalEnum<TimerState>("State");
        filter.Limit = Model.Prompt.UInt("Limit", 100);
        filter.Offset = Model.Prompt.UInt("Offset", 0);

        AnsiConsole.Clear();

        var timers = App.TimerModel.Find(filter);
        var pairs = Model.TimerTaskModel.Pair(timers, App.TaskModel);

        View.TimerView.DisplayTable(pairs);
        Model.Prompt.PressEnter();
    }
}
