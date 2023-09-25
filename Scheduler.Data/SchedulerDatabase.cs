using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sample.data
{
    public class SchedulerDatabase : SQLiteDatabase
    {
        #region Properties

        /// <summary>
        /// Database file
        /// </summary>
        public static readonly string SQLiteDbFile = "scheduler.db";

        /// <summary>
        /// Default database version
        /// </summary>
        public static readonly int SQLiteDbVersion = 3;

        #endregion // Properties

        #region Constructors 

        public SchedulerDatabase() : base(new SQLiteConnectionInfo(SQLiteDbFile, SQLiteDbVersion))
        { }

        public SchedulerDatabase(IsolationLevel level) : base(new SQLiteConnectionInfo(SQLiteDbFile, SQLiteDbVersion), level)
        { }

        #endregion // Constructors 

        #region Methods

        public List<Task> GetTasks()
        {
            DataSet ds = ExecuteReader(new SQLiteCommand("select * from task"));
            return DataTableToList<Task>(GetFirstTable(ds));
        }

        public List<TaskType> GetTaskTypes()
        {
            DataSet ds = ExecuteReader(new SQLiteCommand("select * from TaskType"));
            return DataTableToList<TaskType>(GetFirstTable(ds));
        }

        public List<Alogorithm> GetAlogorithms()
        {
            DataSet ds = ExecuteReader(new SQLiteCommand("select * from Alogorithm"));
            return DataTableToList<Alogorithm>(GetFirstTable(ds));
        }


        public bool Add(Task t)
        {
            SQLiteCommand cmd = new SQLiteCommand(
                @"insert into Task(MediaFile, AlogorithmCode, TaskTypeId, RequestedStart, MaxExecutionSeconds, LatestCompletion) 
                  values (@MediaFile, @AlogorithmCode, @TaskTypeId, @RequestedStart, @MaxExecutionSeconds, @LatestCompletion)");

            SQLiteParameter p1 = new SQLiteParameter(DbType.String) { ParameterName = "@MediaFile", Direction = ParameterDirection.Input, Value = t.MediaFile };
            SQLiteParameter p2 = new SQLiteParameter(DbType.String) { ParameterName = "@AlogorithmCode", Direction = ParameterDirection.Input, Value = t.AlogorithmCode };
            SQLiteParameter p3 = new SQLiteParameter(DbType.Int32) { ParameterName = "@TaskTypeId", Direction = ParameterDirection.Input, Value = t.TaskTypeId };
            SQLiteParameter p4 = new SQLiteParameter(DbType.String) { ParameterName = "@RequestedStart", Direction = ParameterDirection.Input, Value = t.RequestedStart };
            SQLiteParameter p5 = new SQLiteParameter(DbType.Int32) { ParameterName = "@MaxExecutionSeconds", Direction = ParameterDirection.Input, Value = t.MaxExecutionSeconds };
            SQLiteParameter p6 = new SQLiteParameter(DbType.String) { ParameterName = "@LatestCompletion", Direction = ParameterDirection.Input, Value = t.LatestCompletion };

            bool success = ExecuteNonQuery(cmd, new SQLiteParameter[] { p1, p2, p3, p4, p5, p6 }) != 0;

            return success;
        }

        #region Private Methods

        #endregion // Private Methods

        #endregion // Methods
    }
}
