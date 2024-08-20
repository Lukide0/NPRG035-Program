using System;
using SQLite;

namespace task_tracker.Task;

public enum TimerState
{
    Running,
    Paused,
}

[Table("timers")]
public class TimerRecord
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public uint Id { get; set; }

    [Column("task_id")]
    public uint TaskId { get; set; }

    [Column("state")]
    public TimerState State { get; set; } = TimerState.Running;

    [Column("accumulated")]
    public TimeSpan Accumulated { get; set; } = TimeSpan.Zero;

    [Column("start_date")]
    public DateTime? Start { get; set; }
}
