using task_tracker.Cli;
using task_tracker.Presenter;

namespace task_tracker;

class App
{
    private static App? _Instance = null;

    public bool UseColors { get; private set; }
    public bool Verbose { get; private set; }

    public static App GetInstance()
    {
        if (_Instance is null)
        {
            _Instance = new App();
        }
        return _Instance;
    }

    public App()
    {
        UseColors = true;
        Verbose = false;
    }

    public void Run(CliOptions options)
    {
        UseColors = options.NoColor;
        Verbose = options.Verbose;

        IPresenter presenter;
        if (options.Options is null)
        {
            presenter = new MenuPresenter();
        }
        else
        {
            presenter = new CliPresenter(options.Options);
        }

        presenter.Run();
    }
}
