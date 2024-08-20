using System.Collections.Generic;
using task_tracker.Task;

namespace task_tracker.Model;

/// <summary>
/// Represents a model for managing timers.
/// </summary>
public interface ITimerModel
{
    /// <summary>
    /// Attempts to add a new timer to the model.
    /// </summary>
    /// <param name="timer">The timer to add.</param>
    /// <returns>True if the timer was added successfully, false
    /// otherwise.</returns>
    public bool TryAddTimer(TimerRecord record);

    /// <summary>
    /// Removes a timer from the model.
    /// </summary>
    /// <param name="timerId">The ID of the timer to remove.</param>
    /// <returns>True if the timer was removed successfully, false
    /// otherwise.</returns>
    public bool TryRemoveTimer(uint timerID);

    /// <summary>
    /// Updates an existing timer in the model.
    /// </summary>
    /// <param name="timer">The updated timer.</param>
    /// <returns>True if the timer was updated successfully, false
    /// otherwise.</returns>
    public bool TryUpdateTimer(TimerRecord record);

    /// <summary>
    /// Finds a timer by its ID.
    /// </summary>
    /// <param name="timerId">The ID of the timer to find.</param>
    /// <returns>The timer with the specified ID, or null if not found.</returns>
    public TimerRecord? FindByTaskId(uint taskID);

    /// <summary>
    /// Finds running timers
    /// </summary>
    /// <returns>A list of running timers.</returns>
    public List<TimerRecord> FindRunning();

    /// <summary>
    /// Finds timers based on the specified filter options.
    /// </summary>
    /// <param name="options">The filter options to apply.</param>
    /// <returns>A list of timers that match the filter options.</returns>
    public List<TimerRecord> Find(TimerFilterOptions opts);
}
