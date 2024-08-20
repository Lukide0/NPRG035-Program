using Spectre.Console;

namespace task_tracker.View;

/// <summary> View for filtering tasks. </summary>
public class TaskFilterView
{
    /// <summary> Filter menu items. </summary>
    public enum FilterMenuItem { Show, SetFilter, ClearFilter, Back }

    public delegate void HandleFilterMenu(FilterMenuItem item);

    public event HandleFilterMenu? OnFilterMenu;

    /// <summary> Display filter menu. </summary>
    public void Display()
    {
        var selected = Model.Prompt.Enum<FilterMenuItem>("Filter action");
        OnFilterMenu?.Invoke(selected);
    }
}
