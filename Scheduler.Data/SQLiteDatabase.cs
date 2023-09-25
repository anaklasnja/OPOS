using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

namespace sample.data
{
    public class SQLiteDatabase : DataAccess
    {
        public SQLiteDatabase(SQLiteConnectionInfo connectInfo) : base(connectInfo) { }

        /// <summary>
        /// Initialize the object for "transactional" activity.  If the constructor executes successfully,
        /// the connection to the database will be open and the transaction will be started.
        /// </summary>
        /// <param name="connectInfo"></param>
        /// <param name="level"></param>
        protected SQLiteDatabase(SQLiteConnectionInfo connectInfo, IsolationLevel level) : base(connectInfo, level) { }

        #region Methods

        public override IDbConnection GetConnection()
        {
            return new SQLiteConnection(DatabaseConnectionInfo.ConnectionString);
        }

        public int ExecuteNonQuery(string sql, SQLiteParameter[] parms)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql) { CommandType = CommandType.Text };
            return ExecuteNonQuery(cmd, parms);
        }

        public int ExecuteNonQuery(IDbCommand cmd, SQLiteParameter[] parms)
        {
            try
            {
                PrepCommand(cmd, parms);
                if (cmd.Connection.State == ConnectionState.Broken || cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                {
                    if (!RunAsTransaction)
                    {
                        TerminateConnection(cmd.Connection);
                    }
                }
            }
        }

        public DataSet ExecuteReader(SQLiteCommand cmd)
        {
            SQLiteConnection c = null;
            DataSet ds = null;
            try
            {
                c = new SQLiteConnection(DatabaseConnectionInfo.ConnectionString);
                cmd.Connection = c;
                SQLiteDataAdapter a = new SQLiteDataAdapter(cmd);
                ds = new DataSet();
                cmd.Prepare();
                c.Open();
                a.Fill(ds);
                return ds;
            }
            finally
            {
                c?.Close();
            }
        }

        public List<T> DataTableToList<T>(DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        #endregion // Methods
    }
}
