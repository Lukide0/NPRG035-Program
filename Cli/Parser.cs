using System;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

using task_tracker.Task;

namespace task_tracker.Cli;

/// <summary> Command line parser. </summary>
class Parser
{
    /// <summary> Parse command line arguments. </summary>
    /// <param name="args"> Command line arguments. </param>
    /// <returns> Parsed options. </returns>
    public static CliOptions? Parse(string[] args)
    {
        var options = new CliOptions();

        var root = new RootCommand();

        var verboseOpt =
            new Option<bool>(aliases: new string[] { "-v", "--verbose" });
        var colorOpt =
            new Option<bool>(name: "--no-color", getDefaultValue: () => false);

        root.AddGlobalOption(verboseOpt);
        root.AddGlobalOption(colorOpt);

        TaskCmds(root, options);
        TimerCmds(root, options);

        // Allow 0 arguments
        root.SetHandler(() => { });

        var parser =
            new CommandLineBuilder(root)
                .UseDefaults()
                .AddMiddleware((context, next) =>
                {
                    var verbose = context.ParseResult.GetValueForOption(verboseOpt);
                    options.Verbose = verbose;

                    var noColor = context.ParseResult.GetValueForOption(colorOpt);
                    options.NoColor = noColor;

                    return next(context);
                })
                .Build();

        parser.Invoke(args);

        if ((options.Options is null || options.Options.Type == CmdType.Unknown) &&
            args.Count() != 0)
        {
            return null;
        }

        return options;
    }

    /// <summary> Add timer subcommands.</summary>
    private static void TimerCmds(RootCommand root, CliOptions options)
    {
        var timerCmd = new Command("timer");

        root.AddCommand(timerCmd);

        TimerStartCmd(timerCmd, options);
        TimerPauseCmd(timerCmd, options);
        TimerRemoveCmd(timerCmd, options);
        TimerFilterCmd(timerCmd, options);
    }

    /// <summary> Add task subcommands. </summary>
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
        var nameOpt = new Option<string?>("--name");
        var descOpt = new Option<string?>("--description");
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
            options.Options = new EditTaskOptions(id, name, desc, priority, deadline);
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
        var limitOpt = new Option<uint>("--limit", getDefaultValue: () => 100);
        var offsetOpt = new Option<uint>("--offset", getDefaultValue: () => 0);

        filterCmd.AddOption(idOpt);
        filterCmd.AddOption(priorityOpt);
        filterCmd.AddOption(stateOpt);
        filterCmd.AddOption(dateStartOpt);
        filterCmd.AddOption(dateEndOpt);
        filterCmd.AddOption(limitOpt);
        filterCmd.AddOption(offsetOpt);

        root.AddCommand(filterCmd);

        filterCmd.SetHandler(
            (id, priority, state, dateStart, dateEnd, limit, offset) =>
            {
                options.Options = new FilterTaskOptions(new TaskFilterOptions(
                id, priority, state, dateStart, dateEnd, limit, offset));
            },
            idOpt, priorityOpt, stateOpt, dateStartOpt, dateEndOpt, limitOpt,
            offsetOpt);
    }

    private static void TimerStartCmd(Command root, CliOptions options)
    {
        var cmd = new Command("start");
        var taskIdArg = new Argument<uint>("task_id");

        cmd.AddArgument(taskIdArg);

        root.AddCommand(cmd);

        cmd.SetHandler((id) => { options.Options = new StartTimerOptions(id); },
                       taskIdArg);
    }
    private static void TimerPauseCmd(Command root, CliOptions options)
    {
        var cmd = new Command("pause");
        var taskIdArg = new Argument<uint>("task_id");

        cmd.AddArgument(taskIdArg);

        root.AddCommand(cmd);

        cmd.SetHandler(id => { options.Options = new PauseTimerOptions(id); },
                       taskIdArg);
    }
    private static void TimerRemoveCmd(Command root, CliOptions options)
    {
        var cmd = new Command("remove");
        var taskIdArg = new Argument<uint>("task_id");

        cmd.AddArgument(taskIdArg);

        root.AddCommand(cmd);

        cmd.SetHandler(id => { options.Options = new RemoveTimerOptions(id); },
                       taskIdArg);
    }
    private static void TimerFilterCmd(Command root, CliOptions options)
    {

        var cmd = new Command("filter");

        var idOpt = new Option<uint?>("--by-id");
        var stateOpt = new Option<TimerState?>("--by-state");
        var limitOpt = new Option<uint>("--limit", getDefaultValue: () => 100);
        var offsetOpt = new Option<uint>("--offset", getDefaultValue: () => 0);

        cmd.AddOption(idOpt);
        cmd.AddOption(stateOpt);
        cmd.AddOption(limitOpt);
        cmd.AddOption(offsetOpt);

        root.AddCommand(cmd);

        cmd.SetHandler((id, state, limit, offset) =>
        {
            options.Options = new FilterTimerOptions(
                new TimerFilterOptions(id, state, limit, offset));
        }, idOpt, stateOpt, limitOpt, offsetOpt);
    }
}
