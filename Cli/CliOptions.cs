using System;
using task_tracker.Task;

namespace task_tracker.Cli;

/// <summary> Command type. </summary>
public enum CmdType
{
    /// <summary> Unknown command. </summary>
    Unknown,
    /// <summary> Add task. </summary>
    AddTask,
    /// <summary> Edit task. </summary>
    EditTask,
    /// <summary> Remove task. </summary>
    RemoveTask,
    /// <summary> Filter tasks. </summary>
    FilterTasks,
    /// <summary> Start timer. </summary>
    StartTimer,
    /// <summary> Pause timer. </summary>
    PauseTimer,
    /// <summary> Remove timer. </summary>
    RemoveTimer,
    /// <summary> Filter timers. </summary>
    FilterTimer,
}

/// <summary> Base options. </summary>
public record class BaseOptions
(CmdType Type = CmdType.Unknown);

/// <summary> Add task options. </summary>
public record class AddTaskOptions
(string Name, string Description, TaskPriority Priority, DateTime? Deadline)
    : BaseOptions(CmdType.AddTask);

/// <summary> Edit task options. </summary>
public record class EditTaskOptions
(uint Id, string? Name, string? Description, TaskPriority? Priority,
 DateTime? Deadline)
    : BaseOptions(CmdType.EditTask);

/// <summary> Remove task options. </summary>
public record class RemoveTaskOptions
(uint Id) : BaseOptions(CmdType.RemoveTask);

/// <summary> Start timer options. </summary>
public record class StartTimerOptions
(uint taskId) : BaseOptions(CmdType.StartTimer);

/// <summary> Pause timer options. </summary>
public record class PauseTimerOptions
(uint taskId) : BaseOptions(CmdType.PauseTimer);

/// <summary> Remove timer options. </summary>
public record class RemoveTimerOptions
(uint taskId) : BaseOptions(CmdType.RemoveTimer);

/// <summary> Filter task options. </summary>
public record class FilterTaskOptions
(TaskFilterOptions Filter) : BaseOptions(CmdType.FilterTasks);

/// <summary> Filter timer options. </summary>
public record class FilterTimerOptions
(TimerFilterOptions Filter) : BaseOptions(CmdType.FilterTasks);

/// <summary> Cli options. </summary>
public class CliOptions
{
    public bool Verbose;
    public bool NoColor;

    public BaseOptions? Options;
}
