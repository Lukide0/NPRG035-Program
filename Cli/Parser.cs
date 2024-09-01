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

        var root = new RootCommand("Todo application");

        var verboseOpt =
            new Option<bool>(aliases: new string[] { "-v", "--verbose" },
                             description: "Verbose output");
        var colorOpt =
            new Option<bool>(name: "--no-color", getDefaultValue: () => false,
                             description: "No colors");

        root.AddGlobalOption(verboseOpt);
        root.AddGlobalOption(colorOpt);

        var uiCmd = new Command("ui", "Show UI.");
        root.AddCommand(uiCmd);

        uiCmd.SetHandler(() => { options.Options = new UIOptions(); });

        TaskCmds(root, options);
        TimerCmds(root, options);

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

    private static void ValidatorOpt<T>(OptionResult result, Option<T> option)
    {
        var optionName = option.Name;

        try
        {
            var _ = result.GetValueForOption(option);
        }
        catch (System.InvalidOperationException)
        {
            result.ErrorMessage = "Invalid value for option '--" + optionName + "'";
        }
    }

    private static void ValidatorArg<T>(ArgumentResult result, Argument<T> arg)
    {
        var argName = arg.Name;

        try
        {
            var _ = result.GetValueForArgument(arg);
        }
        catch (System.InvalidOperationException)
        {
            result.ErrorMessage = "Invalid value for argument '" + argName + "'";
        }
    }

    private static void AddValidatorOpt<T>(Option<T> opt)
    {
        opt.AddValidator(res => ValidatorOpt<T>(res, opt));
    }

    private static void AddValidatorArg<T>(Argument<T> arg)
    {
        arg.AddValidator(res => ValidatorArg<T>(res, arg));
    }

    /// <summary> Add timer subcommands.</summary>
    private static void TimerCmds(RootCommand root, CliOptions options)
    {
        var timerCmd = new Command("timer", "Timer commands");

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
        var addCmd = new Command("add", "Add task");
        addCmd.AddAlias("new");

        var nameOpt = new Option<string>(
            name: "--name", isDefault: false, parseArgument: result =>
            {
                return result.Tokens.Single().Value;
            }, description: "Task name")
        { IsRequired = true };

        var descOpt =
            new Option<string>(name: "--description", getDefaultValue: () => "",
                               description: "Task description");

        var priorityOpt = new Option<TaskPriority>(
            name: "--priority", getDefaultValue: () => TaskPriority.Medium,
            description: "Task priority");

        var deadlineOpt = new Option<DateTime
            ?>(name: "--deadline", getDefaultValue: () => null,
                 description: "Task deadline");

        AddValidatorOpt(nameOpt);
        AddValidatorOpt(descOpt);
        AddValidatorOpt(priorityOpt);
        AddValidatorOpt(deadlineOpt);

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
        var editCmd = new Command("edit", "Edit task");
        editCmd.AddAlias("update");

        var idArg = new Argument<uint>(name: "task_id", description: "Task ID");
        var nameOpt =
            new Option<string?>("--name", description: ("Task " + "name"));
        var descOpt = new Option<string
            ?>("--description", description: "Task description");
        var priorityOpt = new Option<TaskPriority
            ?>("--priority", description: "Task priority");
        var deadlineOpt = new Option<DateTime
            ?>("--deadline", description: "Task deadline");

        AddValidatorArg(idArg);
        AddValidatorOpt(nameOpt);
        AddValidatorOpt(descOpt);
        AddValidatorOpt(priorityOpt);
        AddValidatorOpt(deadlineOpt);

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
        var removeCmd = new Command("remove", "Remove task");
        removeCmd.AddAlias("delete");

        var idArg = new Argument<uint>(name: "task_id", description: "Task ID");

        AddValidatorArg(idArg);
        removeCmd.AddArgument(idArg);

        root.AddCommand(removeCmd);

        removeCmd.SetHandler(
            (id) => { options.Options = new RemoveTaskOptions(id); }, idArg);
    }

    private static void TaskFilterCmd(RootCommand root, CliOptions options)
    {
        var filterCmd = new Command("filter", "Filter tasks");

        var idOpt =
            new Option<uint?>("--by-id", description: ("Filter by " + "ID"));
        var priorityOpt = new Option<TaskPriority
            ?>("--by-priority", description: "Filter by priority");
        var stateOpt = new Option<TaskState
            ?>("--by-state", description: "Filter by state");
        var dateStartOpt = new Option<DateTime
            ?>("--from-date", description: "Filter by tasks from date");
        var dateEndOpt = new Option<DateTime
            ?>("--to-date", description: "Filter by tasks to date");
        var limitOpt = new Option<uint>("--limit", getDefaultValue: () => 100,
                                        description: "Limit results");
        var offsetOpt = new Option<uint>("--offset", getDefaultValue: () => 0,
                                         description: "Offset results");

        AddValidatorOpt(idOpt);
        AddValidatorOpt(priorityOpt);
        AddValidatorOpt(stateOpt);
        AddValidatorOpt(dateStartOpt);
        AddValidatorOpt(dateEndOpt);
        AddValidatorOpt(limitOpt);
        AddValidatorOpt(offsetOpt);

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
        var cmd = new Command("start", "Start timer");
        var taskIdArg = new Argument<uint>("task_id", description: "Task ID");

        AddValidatorArg(taskIdArg);
        cmd.AddArgument(taskIdArg);

        root.AddCommand(cmd);

        cmd.SetHandler((id) => { options.Options = new StartTimerOptions(id); },
                       taskIdArg);
    }
    private static void TimerPauseCmd(Command root, CliOptions options)
    {
        var cmd = new Command("pause", "Pause timer");
        var taskIdArg = new Argument<uint>("task_id", description: "Task ID");

        AddValidatorArg(taskIdArg);
        cmd.AddArgument(taskIdArg);

        root.AddCommand(cmd);

        cmd.SetHandler(id => { options.Options = new PauseTimerOptions(id); },
                       taskIdArg);
    }
    private static void TimerRemoveCmd(Command root, CliOptions options)
    {
        var cmd = new Command("remove", "Remove timer");
        var taskIdArg = new Argument<uint>("task_id", description: "Task ID");

        AddValidatorArg(taskIdArg);
        cmd.AddArgument(taskIdArg);

        root.AddCommand(cmd);

        cmd.SetHandler(id => { options.Options = new RemoveTimerOptions(id); },
                       taskIdArg);
    }
    private static void TimerFilterCmd(Command root, CliOptions options)
    {

        var cmd = new Command("filter", "Filter timers");

        var idOpt =
            new Option<uint?>("--by-id", description: ("Filter by " + "ID"));
        var stateOpt = new Option<TimerState
            ?>("--by-state", description: "Filter by state");
        var limitOpt = new Option<uint>("--limit", getDefaultValue: () => 100,
                                        description: "Limit results");
        var offsetOpt = new Option<uint>("--offset", getDefaultValue: () => 0,
                                         description: "Offset results");

        AddValidatorOpt(idOpt);
        AddValidatorOpt(stateOpt);
        AddValidatorOpt(limitOpt);
        AddValidatorOpt(offsetOpt);

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
