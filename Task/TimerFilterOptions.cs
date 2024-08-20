namespace task_tracker.Task;

/// <summary> Filter options for timer. </summary>
public class TimerFilterOptions
{
    public uint? Id { get; set; }
    public TimerState? State { get; set; }
    public uint Limit { get; set; }
    public uint Offset { get; set; }

    public TimerFilterOptions(uint? id = null, TimerState? state = null,
                              uint limit = 100, uint offset = 0)
    {
        Id = id;
        State = state;
        Limit = limit;
        Offset = offset;
    }
}
