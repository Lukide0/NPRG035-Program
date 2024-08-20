using System;
using System.Collections.Generic;
using task_tracker.Task;

namespace task_tracker.Model;

/// <summary> Task model. </summary>
public interface ITaskModel
{
    /// <summary> Adds a new task. </summary>
    /// <param name="record"> Task record.</param>
    /// <returns> Success.</returns>
    public bool TryAddTask(TaskRecord record);

    /// <summary> Removes a task. </summary>
    /// <param name="taskID"> Task ID.</param>
    /// <returns> Success.</returns>
    public bool TryRemoveTask(uint taskID);

    /// <summary> Updates a task. </summary>
    /// <param name="record"> Task record.</param>
    /// <returns> Success.</returns>
    public bool TryUpdateTask(TaskRecord record);

    /// <summary> Finds a task. </summary>
    /// <param name="taskID"> Task ID.</param>
    /// <returns> Task record.</returns>
    public TaskRecord? FindById(uint taskID);

    /// <summary> Finds tasks. </summary>
    /// <param name="options"> Task filter options.</param>
    /// <returns> Task records.</returns>
    public List<TaskRecord> Find(TaskFilterOptions options);
}
