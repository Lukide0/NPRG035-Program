using Spectre.Console;
using System;
using task_tracker.Task;
using task_tracker.View;

namespace task_tracker.Presenter;

/// <summary> Presenter for filtering tasks through the UI. </summary>
public class UITaskFilterPresenter : IPresenter
{

    private TaskFilterOptions _Filter = new();
    private bool _Run = false;

    public void Run()
    {
        _Run = true;
        var view = new View.TaskFilterView();

        view.OnFilterMenu += HandleMenu;
        while (_Run)
        {
            AnsiConsole.Clear();
            view.Display();
        }
        _Run = false;
    }

    private void HandleMenu(TaskFilterView.FilterMenuItem item)
    {
        switch (item)
        {
            case TaskFilterView.FilterMenuItem.Show:
                ShowTable();
                break;
            case TaskFilterView.FilterMenuItem.SetFilter:
                SetFilter();
                break;
            case TaskFilterView.FilterMenuItem.ClearFilter:
                _Filter = new();
                break;
            case TaskFilterView.FilterMenuItem.Back:
                _Run = false;
                break;
        }
    }

    private void ShowTable()
    {
        var records = App.TaskModel.Find(_Filter);

        View.TaskView.DisplayTable(records);
        Model.Prompt.PressEnter();
    }

    private void SetFilter()
    {
        AnsiConsole.Write(new Rule("Filter").LeftJustified());
        _Filter = Model.Prompt.TaskFilter();
    }
}
