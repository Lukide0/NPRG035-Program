namespace task_tracker.Task;

/// <summary> Task checker. </summary>
public static class TaskChecker
{

    /// <summary> Check task name and description. </summary>
    /// <param name="task"> Task to check.</param>
    /// <returns> True if name and description are valid.</returns>
    public static bool Check(TaskRecord task)
    {
        return Check(task.Name) && task.Name.Length > 0 && Check(task.Description);
    }

    /// <summary> Check string. </summary>
    /// <param name="value"> Value to check.</param>
    /// <returns> True if string is valid.</returns>
    public static bool Check(string value)
    {
        return !value.Contains(';') && !value.Contains('\n');
    }
}
