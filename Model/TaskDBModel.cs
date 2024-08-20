using SQLite;
using task_tracker.Task;
using System;
using System.Collections.Generic;

namespace task_tracker.Model
{

    public class TaskDBModel : ITaskModel
    {
        private SQLiteConnection _Con;

        public TaskDBModel(SQLiteConnection con) { _Con = con; }

        public bool TryAddTask(TaskRecord record)
        {
            var result = _Con.Insert(record);
            return result == 1;
        }
        public bool TryRemoveTask(uint taskID)
        {
            _Con.BeginTransaction();

            var result = _Con.Delete<TaskRecord>(taskID);

            if (result != 1)
            {
                _Con.Commit();
                return false;
            }

            // Remove associated timer
            var timer = App.TimerModel.FindByTaskId(taskID);
            if (timer is not null)
            {
                if (!App.TimerModel.TryRemoveTimer(timer.Id))
                {
                    _Con.Rollback();
                    return false;
                }
            }

            _Con.Commit();
            return true;
        }

        public bool TryUpdateTask(TaskRecord record)
        {
            _Con.BeginTransaction();

            var result = _Con.Update(record);

            if (result != 1)
            {
                _Con.Commit();
                return false;
            }

            if (record.State == TaskState.InProgress)
            {
                _Con.Commit();
                return true;
            }

            var timer = App.TimerModel.FindByTaskId(record.Id);
            if (timer is null || timer.State == TimerState.Paused)
            {
                _Con.Commit();
                return true;
            }

            timer.State = TimerState.Paused;
            if (!App.TimerModel.TryUpdateTimer(timer))
            {
                _Con.Rollback();
                return false;
            }

            return true;
        }

        public TaskRecord? FindById(uint taskID)
        {
            var records =
                _Con.Query<TaskRecord>("SELECT * FROM tasks WHERE id=?", taskID);

            return records.Count == 1 ? records[0] : null;
        }

        public List<TaskRecord> Find(TaskFilterOptions opts)
        {
            List<string> conds = new();

            if (opts.Id is not null)
            {
                conds.Add($"id={opts.Id}");
            }

            if (opts.Priority is not null)
            {
                conds.Add($"priority={(int)opts.Priority}");
            }

            if (opts.State is not null)
            {
                conds.Add($"state = {(int)opts.State}");
            }

            if (opts.DateStart is not null)
            {
                conds.Add($"deadline >= {opts.DateStart.Value.Ticks}");
            }

            if (opts.DateEnd is not null)
            {
                conds.Add($"deadline <= {opts.DateEnd.Value.Ticks}");
            }

            string query = "SELECT * FROM tasks";

            if (conds.Count > 0)
            {
                query += " WHERE ";
                query += String.Join(" AND ", conds);
            }

            query += $" LIMIT {opts.Limit} OFFSET {opts.Offset}";

            return _Con.Query<TaskRecord>(query);
        }
    }

}
