using System;
using SQLite;

namespace task_tracker.Task;

[Table("tasks")]
public class TaskRecord
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public uint Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("priority")]
    public TaskPriority Priority { get; set; }

    [Column("state")]
    public TaskState State { get; set; } = TaskState.ToDo;

    [Column("deadline")]
    public DateTime? Deadline { get; set; }
};
