using task_tracker.Cli;

namespace task_tracker.Presenter;

public class CliTaskFilterPresenter : IPresenter
{
    private FilterTaskOptions _Opts;

    public CliTaskFilterPresenter(FilterTaskOptions opts) { _Opts = opts; }

    public void Run() { }
}
