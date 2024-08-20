using SQLite;
using System.IO;
using System.Diagnostics;
using task_tracker.Task;
using System;
using System.Collections.Generic;

namespace task_tracker.Model
{

    public class TimerDBModel : ITimerModel
    {
        private SQLiteConnection _Con;

        public TimerDBModel(SQLiteConnection con) { _Con = con; }

        public bool TryAddTimer(TimerRecord record)
        {
            var result = _Con.Insert(record);
            return result == 1;
        }
        public bool TryRemoveTimer(uint timerID)
        {
            var result = _Con.Delete<TimerRecord>(timerID);
            return result == 1;
        }

        public bool TryUpdateTimer(TimerRecord record)
        {
            var result = _Con.Update(record);
            return result == 1;
        }

        public List<TimerRecord> FindRunning()
        {
            return _Con.Table<TimerRecord>()
                .Where(t => t.State == TimerState.Running)
                .ToList();
        }

        public TimerRecord? FindByTaskId(uint taskID)
        {
            var records =
                _Con.Query<TimerRecord>("SELECT * FROM timers WHERE task_id=?", taskID);

            return records.Count == 1 ? records[0] : null;
        }

        public List<TimerRecord> Find(TimerFilterOptions opts)
        {
            List<string> conds = new();

            if (opts.Id is not null)
            {
                conds.Add($"id={opts.Id}");
            }

            if (opts.State is not null)
            {
                conds.Add($"state = {(int)opts.State}");
            }

            string query = "SELECT * FROM timers";

            if (conds.Count > 0)
            {
                query += " WHERE ";
                query += String.Join(" AND ", conds);
            }

            query += $" LIMIT {opts.Limit} OFFSET {opts.Offset}";

            return _Con.Query<TimerRecord>(query);
        }
    }

}
