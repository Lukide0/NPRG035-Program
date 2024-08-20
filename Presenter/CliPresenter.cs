using task_tracker.Cli;

namespace task_tracker.Presenter;

/// <summary> Presenter for handling CLI operations. </summary>
public class CliPresenter : IPresenter
{
    protected BaseOptions _Options;

    public CliPresenter(BaseOptions opts) { _Options = opts; }

    public void Run()
    {
        IPresenter presenter;

        switch (_Options)
        {
            case AddTaskOptions opts:
                presenter = new CliTaskAddPresenter(opts);
                break;
            case RemoveTaskOptions opts:
                presenter = new CliTaskRemovePresenter(opts);
                break;
            case EditTaskOptions opts:
                presenter = new CliTaskEditPresenter(opts);
                break;
            case FilterTaskOptions opts:
                presenter = new CliTaskFilterPresenter(opts);
                break;
            case StartTimerOptions opts:
                presenter = new CliTimerStartPresenter(opts);
                break;
            case PauseTimerOptions opts:
                presenter = new CliTimerPausePresenter(opts);
                break;
            case FilterTimerOptions opts:
                presenter = new CliTimerFilterPresenter(opts);
                break;
            default:
                return;
        }

        presenter.Run();
    }
}
