using System;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

using task_tracker.Task;

namespace task_tracker.Cli;

class Parser
{
    public static CliOptions Parse(string[] args)
    {
        var options = new CliOptions();

        var root = new RootCommand();

        var verboseOpt =
            new Option<bool>(aliases: new string[] { "-v", "--verbose" });
        var colorOpt = new Option<bool>(name: "--no-color");

        root.AddGlobalOption(verboseOpt);
        root.AddOption(colorOpt);

        TaskCmds(root, options);
        StopwatchCmds(root, options);

        root.SetHandler((noColor) => { options.NoColor = noColor; }, colorOpt);

        var parser = new CommandLineBuilder(root)
                         .UseDefaults()
                         .AddMiddleware((context, next) =>
                         {
                             var verbose =
                             context.ParseResult.GetValueForOption(verboseOpt);
                             options.Verbose = verbose;

                             return next(context);
                         })
                         .Build();

        parser.Invoke(args);

        return options;
    }

    private static void StopwatchCmds(RootCommand root, CliOptions options) { }

    private static void TaskCmds(RootCommand root, CliOptions options)
    {
        TaskAddCmd(root, options);
        TaskEditCmd(root, options);
        TaskRemoveCmd(root, options);
        TaskFilterCmd(root, options);
    }

    private static void TaskAddCmd(RootCommand root, CliOptions options)
    {
        var addCmd = new Command("add");
        addCmd.AddAlias("new");

        var nameOpt = new Option<string>(name: "--name", isDefault: false,
                                         parseArgument: result =>
                                         {
                                             return result.Tokens.Single().Value;
                                         })
        { IsRequired = true };

        var descOpt =
            new Option<string>(name: "--description", getDefaultValue: () => "");

        var priorityOpt = new Option<TaskPriority>(
            name: "--priority", getDefaultValue: () => TaskPriority.Medium);

        var deadlineOpt = new Option<DateTime
            ?>(name: "--deadline", getDefaultValue: () => null);

        addCmd.AddOption(nameOpt);
        addCmd.AddOption(descOpt);
        addCmd.AddOption(priorityOpt);
        addCmd.AddOption(deadlineOpt);

        root.AddCommand(addCmd);

        addCmd.SetHandler((name, desc, priority, deadline) =>
        {
            options.Options = new AddTaskOptions(name, desc, priority, deadline);
        }, nameOpt, descOpt, priorityOpt, deadlineOpt);
    }

    private static void TaskEditCmd(RootCommand root, CliOptions options)
    {
        var editCmd = new Command("edit");
        editCmd.AddAlias("update");

        var idArg = new Argument<uint>(name: "task_id");

        var nameOpt = new Option<string?>(name: "--name");
        var descOpt = new Option<string?>(name: "--description");
        var priorityOpt = new Option<TaskPriority?>("--priority");
        var deadlineOpt = new Option<DateTime?>("--deadline");

        editCmd.AddArgument(idArg);
        editCmd.AddOption(nameOpt);
        editCmd.AddOption(descOpt);
        editCmd.AddOption(priorityOpt);
        editCmd.AddOption(deadlineOpt);

        root.AddCommand(editCmd);

        editCmd.SetHandler((id, name, desc, priority, deadline) =>
        {
            options.Options = new EditTaskOptions(name, desc, priority, deadline);
        }, idArg, nameOpt, descOpt, priorityOpt, deadlineOpt);
    }

    private static void TaskRemoveCmd(RootCommand root, CliOptions options)
    {
        var removeCmd = new Command("remove");
        removeCmd.AddAlias("delete");

        var idArg = new Argument<uint>(name: "task_id");

        removeCmd.AddArgument(idArg);

        root.AddCommand(removeCmd);

        removeCmd.SetHandler(
            (id) => { options.Options = new RemoveTaskOptions(id); }, idArg);
    }

    private static void TaskFilterCmd(RootCommand root, CliOptions options)
    {
        var filterCmd = new Command("filter");

        var idOpt = new Option<uint?>("--by-id");
        var priorityOpt = new Option<TaskPriority?>("--by-priority");
        var stateOpt = new Option<TaskState?>("--by-state");
        var dateStartOpt = new Option<DateTime?>("--from-date");
        var dateEndOpt = new Option<DateTime?>("--to-date");

        filterCmd.AddOption(idOpt);
        filterCmd.AddOption(priorityOpt);
        filterCmd.AddOption(stateOpt);
        filterCmd.AddOption(dateStartOpt);
        filterCmd.AddOption(dateEndOpt);

        root.AddCommand(filterCmd);

        filterCmd.SetHandler((id, priority, state, dateStart, dateEnd) =>
        {
            options.Options =
                new FilterTaskOptions(id, priority, state, dateStart, dateEnd);
        }, idOpt, priorityOpt, stateOpt, dateStartOpt, dateEndOpt);
    }
}
