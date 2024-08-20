using Spectre.Console;

namespace task_tracker.View;

/// <summary> View for displaying menu. </summary>
public class MenuView
{
    /// <summary> Menu items. </summary>
    public enum MainItem { Tasks, Timers, Quit }
    /// <summary> Task menu items. </summary>
    public enum TasksItem { View, Add, Edit, Back }
    /// <summary> Timer menu items. </summary>
    public enum TimersItem { View, Start, Pause, Remove, Back }

    public delegate void HandleTasksMenu(TasksItem item);
    public delegate void HandleMainMenu(MainItem item);
    public delegate void HandleTimersMenu(TimersItem item);

    public event HandleMainMenu? OnMainMenu;
    public event HandleTasksMenu? OnTasksMenu;
    public event HandleTimersMenu? OnTimersMenu;

    /// <summary> Display main menu. </summary>
    public void DisplayMainMenu()
    {
        var mainPrompt =
            new SelectionPrompt<MainItem>().Title("Action:").AddChoices(new[] {
          MainItem.Tasks,
          MainItem.Timers,
          MainItem.Quit,
            });

        var selected = AnsiConsole.Prompt(mainPrompt);

        OnMainMenu?.Invoke(selected);
    }

    /// <summary> Display tasks menu. </summary>
    public void DisplayTasksMenu()
    {
        var tasksPrompt = new SelectionPrompt<TasksItem>()
                              .Title("Tasks action:")
                              .AddChoices(new[] {
                            TasksItem.View,
                            TasksItem.Add,
                            TasksItem.Edit,
                            TasksItem.Back,
                              });
        var selected = AnsiConsole.Prompt(tasksPrompt);

        OnTasksMenu?.Invoke(selected);
    }

    /// <summary> Display timers menu. </summary>
    public void DisplayTimersMenu()
    {
        var timersPrompt = new SelectionPrompt<TimersItem>()
                               .Title("Timers action:")
                               .AddChoices(new[] {
                             TimersItem.View,
                             TimersItem.Start,
                             TimersItem.Pause,
                             TimersItem.Remove,
                             TimersItem.Back,
                               });

        var selected = AnsiConsole.Prompt(timersPrompt);

        OnTimersMenu?.Invoke(selected);
    }
}
