using task_tracker.Cli;

namespace task_tracker.Presenter;

public class CliTaskAddPresenter : IPresenter
{
    private AddTaskOptions _Opts;

    public CliTaskAddPresenter(AddTaskOptions opts) { _Opts = opts; }

    public void Run() { }
}
