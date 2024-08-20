using Spectre.Console;
using task_tracker.Task;
using task_tracker.View;

namespace task_tracker.Presenter;

/// <summary> Presenter for editing tasks through the UI. </summary>
public class UITaskEditPresenter : IPresenter
{

    public void Run()
    {
        var id = Model.Prompt.UInt("Task id");

        var record = App.TaskModel.FindById(id);

        if (record is null)
        {
            Logger.Err("Task does not exist");
            Model.Prompt.PressEnter();
            return;
        }

        var view = new TaskEditView();
        view.OnEditMenu += (item) =>
        {
            switch (item)
            {
                case TaskEditView.EditMenuItem.Edit:
                    EditTask(record);
                    break;
                case TaskEditView.EditMenuItem.Remove:
                    RemoveTask(record.Id);
                    break;
                case TaskEditView.EditMenuItem.Cancel:
                    return;
            }
        };

        AnsiConsole.Clear();
        view.Display(record);
    }

    private void EditTask(TaskRecord record)
    {

        if (!AnsiConsole.Confirm("Edit this task?", false))
        {
            return;
        }

        var newRecord = new TaskRecord();
        newRecord.Name =
            Model.Prompt.String("Name", "[red]Name cannot contain ';' or '\\n'[/]",
                                Task.TaskChecker.Check);
        newRecord.Description = Model.Prompt.String(
            "Description", "[red]Name cannot contain ';' or '\\n'[/]",
            Task.TaskChecker.Check, true);
        newRecord.Priority = Model.Prompt.Enum<TaskPriority>("Priority");
        newRecord.State = Model.Prompt.Enum<TaskState>("State");
        newRecord.Deadline = Model.Prompt.OptionalDate("Deadline");

        AnsiConsole.Clear();

        AnsiConsole.Write(new Rule("before"));
        TaskView.Display(record);

        AnsiConsole.Write(new Rule("after"));
        TaskView.Display(newRecord);

        if (!AnsiConsole.Confirm("Save changes?"))
        {
            return;
        }

        // Set primary key
        newRecord.Id = record.Id;

        if (!App.TaskModel.TryUpdateTask(newRecord))
        {
            Logger.Err("Cannot edit task");
            Model.Prompt.PressEnter();
        }
    }

    private void RemoveTask(uint id)
    {
        if (!AnsiConsole.Confirm("Remove this task?", false))
        {
            return;
        }

        if (!App.TaskModel.TryRemoveTask(id))
        {
            Logger.Err("Cannot remove task");
            Model.Prompt.PressEnter();
        }
    }
}
