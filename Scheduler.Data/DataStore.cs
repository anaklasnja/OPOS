using System.Data;

namespace sample.data
{
    public class DataStore
    {
        private static DataStore _instance = null;

        private DataStore()
        {
            Db = new SchedulerDatabase();
        }

        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataStore();
                }

                return _instance;
            }
        }

        public static SchedulerDatabase NewTran() { return new SchedulerDatabase(IsolationLevel.ReadCommitted); }

        public SchedulerDatabase Db { get; }
    }
}
