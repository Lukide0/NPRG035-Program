using task_tracker.Cli;

namespace task_tracker;

class Program
{
    static void Main(string[] args)
    {
        CliOptions? options = Parser.Parse(args);

        if (options is null)
        {
            return;
        }

        App.GetInstance().Run(options);
    }
}
