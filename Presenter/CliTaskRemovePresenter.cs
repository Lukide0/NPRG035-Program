using task_tracker.Cli;

namespace task_tracker.Presenter;

public class CliTaskRemovePresenter : IPresenter
{
    private RemoveTaskOptions _Opts;

    public CliTaskRemovePresenter(RemoveTaskOptions opts) { _Opts = opts; }

    public void Run() { }
}
