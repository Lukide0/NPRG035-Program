using System.Diagnostics;
using task_tracker.Cli;

namespace task_tracker.Presenter;

/// <summary> Presenter for removing tasks through the CLI. </summary>
public class CliTaskRemovePresenter : IPresenter
{
    private RemoveTaskOptions _Opts;

    public CliTaskRemovePresenter(RemoveTaskOptions opts) { _Opts = opts; }

    public void Run()
    {
        Debug.Assert(App.TaskModel is not null);

        Logger.Info($"Removing task with id '{_Opts.Id}'");
        if (!App.TaskModel.TryRemoveTask(_Opts.Id))
        {
            Logger.Err("Cannot remove task");
            return;
        }

        Logger.Info("Task removed");
    }
}
