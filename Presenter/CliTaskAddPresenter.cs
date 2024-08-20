using System.Diagnostics;
using task_tracker.Cli;
using task_tracker.Task;

namespace task_tracker.Presenter;

/// <summary> Presenter for adding tasks through the CLI. </summary>
public class CliTaskAddPresenter : IPresenter
{
    private AddTaskOptions _Opts;

    public CliTaskAddPresenter(AddTaskOptions opts) { _Opts = opts; }

    public void Run()
    {

        Debug.Assert(App.TaskModel is not null);

        var record =
            new TaskRecord()
            {
                Name = _Opts.Name,
                Description = _Opts.Description,
                Priority = _Opts.Priority,
                Deadline = _Opts.Deadline
            };

        Logger.Info("Checking task name and description");
        if (!TaskChecker.Check(record))
        {
            Logger.Err("Task name and description cannot contain newline or ';'");
            return;
        }

        Logger.Info("Adding task");
        if (!App.TaskModel.TryAddTask(record))
        {
            Logger.Err("Cannot add task");
            return;
        }

        Logger.Info($"Task added with ID: {record.Id}");
    }
}
