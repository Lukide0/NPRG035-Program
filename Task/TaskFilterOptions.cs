using System;

namespace task_tracker.Task;

/// <summary> Options for filtering tasks. </summary>
public class TaskFilterOptions
{
    public uint? Id { get; set; }
    public TaskPriority? Priority { get; set; }
    public TaskState? State { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public uint Limit { get; set; }
    public uint Offset { get; set; }

    public TaskFilterOptions(uint? id = null, TaskPriority? priority = null,
                             TaskState? state = null, DateTime? dateStart = null,
                             DateTime? dateEnd = null, uint limit = 100,
                             uint offset = 0)
    {
        Id = id;
        Priority = priority;
        State = state;
        DateStart = dateStart;
        DateEnd = dateEnd;
        Limit = limit;
        Offset = offset;
    }
}
