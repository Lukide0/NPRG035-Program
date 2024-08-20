using System;
using Spectre.Console;
using task_tracker.Task;

namespace task_tracker.Presenter;

/// <summary> Presenter for adding tasks through the UI. </summary>
public class UITaskAddPresenter : IPresenter
{
    private TaskRecord _NewRecord = new();

    public void Run()
    {

        _NewRecord = new();
        _NewRecord.Name = PromptName();
        _NewRecord.Description = PromptDescription();
        _NewRecord.Priority = PromptPriority();
        _NewRecord.State = PromptState();
        _NewRecord.Deadline = PromptDeadline();

        if (!Confirm())
        {
            return;
        }

        AnsiConsole.Clear();
        if (!App.TaskModel.TryAddTask(_NewRecord))
        {
            AnsiConsole.Write(
                new Markup("[red bold]ERROR:[/] Cannot create new task"));
        }
        else
        {
            AnsiConsole.Write(
                new Markup($"[green bold]Created task with id '{_NewRecord.Id}'[/]"));
        }

        AnsiConsole.WriteLine();
        Model.Prompt.PressEnter();
    }

    private void PrintHeader()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[yellow]New task[/]").LeftJustified());
    }

    private void PrintNewTask() { View.TaskView.Display(_NewRecord); }

    private string PromptName()
    {
        PrintHeader();

        return Model.Prompt.String("Name",
                                   "[red]Name cannot contain ';' or '\\n'[/]",
                                   Task.TaskChecker.Check);
    }

    private string PromptDescription()
    {
        PrintHeader();
        return Model.Prompt.String("Description",
                                   "[red]Name cannot contain ';' or '\\n'[/]",
                                   Task.TaskChecker.Check, true);
    }

    private TaskPriority PromptPriority()
    {
        PrintHeader();
        return Model.Prompt.Enum<TaskPriority>("Priority");
    }

    private TaskState PromptState()
    {
        PrintHeader();
        return Model.Prompt.Enum<TaskState>("State");
    }

    private DateTime? PromptDeadline()
    {
        PrintHeader();
        return Model.Prompt.OptionalDate("Deadline");
    }

    private bool Confirm()
    {
        PrintHeader();
        PrintNewTask();

        return AnsiConsole.Prompt(new ConfirmationPrompt("Create new task?"));
    }
}
