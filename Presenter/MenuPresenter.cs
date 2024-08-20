using task_tracker.View;
using Spectre.Console;
using System;

namespace task_tracker.Presenter;

/// <summary> Presenter for handling menu operations. </summary>
public class MenuPresenter : IPresenter
{
    private MenuView _Menu = new MenuView();
    private bool _Run = false;

    private delegate void Display();

    private Display? _CurrentMenu;

    public MenuPresenter()
    {
        _Menu.OnMainMenu += HandleMainMenu;
        _Menu.OnTasksMenu += HandleTasksMenu;
        _Menu.OnTimersMenu += HandleTimersMenu;
    }

    public void Run()
    {

        Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
        {
            // HACK: When in alternate screen mode, this will exit the mode after
            // execution.
            AnsiConsole.AlternateScreen(() => { });
        };

        AnsiConsole.AlternateScreen(RunUI);
    }

    private void RunUI()
    {
        _Run = true;
        _CurrentMenu = _Menu.DisplayMainMenu;
        while (_Run && _CurrentMenu is not null)
        {
            AnsiConsole.Clear();
            _CurrentMenu.Invoke();
        }

        _CurrentMenu = null;
        _Run = false;
    }

    private void HandleMainMenu(MenuView.MainItem item)
    {
        switch (item)
        {
            case MenuView.MainItem.Quit:
                _CurrentMenu = null;
                _Run = false;
                break;
            case MenuView.MainItem.Tasks:
                _CurrentMenu = _Menu.DisplayTasksMenu;
                break;
            case MenuView.MainItem.Timers:
                _CurrentMenu = _Menu.DisplayTimersMenu;
                break;
        }
    }
    private void HandleTasksMenu(MenuView.TasksItem item)
    {
        IPresenter? presenter = null;

        switch (item)
        {
            case MenuView.TasksItem.Add:
                presenter = new UITaskAddPresenter();
                break;
            case MenuView.TasksItem.View:
                presenter = new UITaskFilterPresenter();
                break;
            case MenuView.TasksItem.Edit:
                presenter = new UITaskEditPresenter();
                break;
            case MenuView.TasksItem.Back:
                _CurrentMenu = _Menu.DisplayMainMenu;
                return;
        }

        presenter?.Run();
    }
    private void HandleTimersMenu(MenuView.TimersItem item)
    {
        IPresenter? presenter = null;

        switch (item)
        {
            case MenuView.TimersItem.View:
                presenter = new UITimerFilterPresenter();
                break;
            case MenuView.TimersItem.Start:
                presenter = new UITimerStartPresenter();
                break;
            case MenuView.TimersItem.Pause:
                presenter = new UITimerPausePresenter();
                break;
            case MenuView.TimersItem.Remove:
                presenter = new UITimerRemovePresenter();
                break;
            case MenuView.TimersItem.Back:
                _CurrentMenu = _Menu.DisplayMainMenu;
                return;
        }

        presenter?.Run();
    }
}
