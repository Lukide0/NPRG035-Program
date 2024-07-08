using System;
using task_tracker.Task;

namespace task_tracker.Cli;

public enum CmdType
{
    Unknown,
    AddTask,
    EditTask,
    RemoveTask,
    FilterTasks,
}

public record class BaseOptions
(CmdType Type = CmdType.Unknown);

public record class AddTaskOptions
(string Name, string Description, TaskPriority Priority, DateTime? Deadline)
    : BaseOptions(CmdType.AddTask);

public record class EditTaskOptions
(string? Name, string? Description, TaskPriority? Priority, DateTime? Deadline)
    : BaseOptions(CmdType.EditTask);

public record class RemoveTaskOptions
(uint Id) : BaseOptions(CmdType.RemoveTask);

public record class FilterTaskOptions
(uint? Id, TaskPriority? Priority, TaskState? State, DateTime? DateStart,
 DateTime? DateEnd)
    : BaseOptions(CmdType.FilterTasks);

public class CliOptions
{
    public bool Verbose;
    public bool NoColor;

    public BaseOptions? Options;
}
