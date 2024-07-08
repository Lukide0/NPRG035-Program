using task_tracker.Cli;

namespace task_tracker.Presenter;

public class CliTaskEditPresenter : IPresenter
{
    private EditTaskOptions _Opts;

    public CliTaskEditPresenter(EditTaskOptions opts) { _Opts = opts; }

    public void Run() { }
}
