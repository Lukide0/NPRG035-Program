using System.Diagnostics;
using task_tracker.Cli;

namespace task_tracker.Presenter;

/// <summary> Presenter for editing tasks through the CLI. </summary>
public class CliTaskEditPresenter : IPresenter
{
    private EditTaskOptions _Opts;

    public CliTaskEditPresenter(EditTaskOptions opts) { _Opts = opts; }

    public void Run()
    {
        Debug.Assert(App.TaskModel is not null);

        var record = App.TaskModel.FindById(_Opts.Id);

        if (record is null)
        {
            Logger.Err($"Task with id '{_Opts.Id}' does not exists");
            return;
        }

        Logger.Info("Editing task");

        record.Name = _Opts.Name ?? record.Name;
        record.Priority = _Opts.Priority ?? record.Priority;
        record.Description = _Opts.Description ?? record.Description;
        record.Deadline = _Opts.Deadline ?? record.Deadline;

        if (!App.TaskModel.TryUpdateTask(record))
        {
            Logger.Err("Cannot edit task");
            return;
        }

        Logger.Info("Task edited");
    }
}
