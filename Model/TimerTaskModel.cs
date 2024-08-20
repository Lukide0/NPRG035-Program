using System.Collections.Generic;
using task_tracker.Task;

namespace task_tracker.Model;

/// <summary>
/// Represents a record of timer and parent task.
/// </summary>
public record class TimerTaskRecord
(TimerRecord Timer, TaskRecord Task);

/// <summary> Timer task model. </summary>
public static class TimerTaskModel
{
    /// <summary>
    /// Pairs timers with their corresponding tasks.
    /// </summary>
    /// <param name="timers">The collection of timer records to pair.</param>
    /// <param name="taskModel">The task model to use for pairing.</param>
    /// <returns>A list of paired timer task records.</returns>
    public static List<TimerTaskRecord> Pair(IEnumerable<TimerRecord> timers,
                                             ITaskModel taskModel)
    {
        List<TimerTaskRecord> records = new();

        foreach (var timer in timers)
        {
            var task = taskModel.FindById(timer.TaskId);
            if (task is null)
            {
                continue;
            }

            records.Add(new TimerTaskRecord(timer, task));
        }

        return records;
    }
}
