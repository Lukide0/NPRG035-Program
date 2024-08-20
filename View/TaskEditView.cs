using Spectre.Console;
using task_tracker.Task;

namespace task_tracker.View;

/// <summary> View for editing tasks. </summary>
public class TaskEditView
{
    /// <summary> Edit menu items. </summary>
    public enum EditMenuItem { Edit, Remove, Cancel }

    public delegate void HandleEditMenu(EditMenuItem item);

    public event HandleEditMenu? OnEditMenu;

    public void Display(TaskRecord record)
    {
        AnsiConsole.Write(new Rule("Task"));
        TaskView.Display(record);
        AnsiConsole.Write(new Rule());
        var selected = Model.Prompt.Enum<EditMenuItem>("Action");
        OnEditMenu?.Invoke(selected);
    }
}
