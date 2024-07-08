using task_tracker.Cli;

namespace task_tracker;

class Program
{
    static void Main(string[] args)
    {
        CliOptions options = Parser.Parse(args);

        App.GetInstance().Run(options);
    }
}
