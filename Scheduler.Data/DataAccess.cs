using System;
using System.Data;

namespace sample.data
{
    public abstract class DataAccess
    {
        #region Constants

        /// <summary>
        /// Timeout for SQL operations.  Set to 5 minutes.
        /// </summary>
        protected const int DefaultCommandTimeout = 300;

        #endregion // Constants 

        #region Constructors 

        /// <summary>
        /// Initialize the object with the information for the SQL Server database.
        /// </summary>
        /// <param name="info">SQL information.</param>
        protected DataAccess(DatabaseInfo info)
        {
            DatabaseConnectionInfo = info ?? throw new ArgumentNullException(nameof(info));
            CommandTimeout = DefaultCommandTimeout;
        }

        /// <summary>
        /// Initialize the object for "transactional" activity.  If the constructor executes successfully,
        /// the connection to the database will be open and the transaction will be started.
        /// </summary>
        /// <param name="connectInfo"></param>
        /// <param name="level"></param>
        protected DataAccess(DatabaseInfo connectInfo, IsolationLevel level) : this(connectInfo)
        {
            // The following will open up the connection to the database and
            // start the transaction.
            Transaction = StartTransaction(level);
            RunAsTransaction = true;
        }

        #endregion // Constructors 

        #region Properties

        /// <summary>
        /// Timeout for execution of SQL commands
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// Get database information.
        /// </summary>
        public DatabaseInfo DatabaseConnectionInfo { get; }

        /// <summary>
        /// Get the object representing connection to the database.
        /// </summary>
        /// <returns></returns>
        public abstract IDbConnection GetConnection();

        /// <summary>
        /// Start the transaction with the specified Isolation level
        /// </summary>
        /// <param name="level">Isolation Level</param>
        /// <exception cref="InvalidOperationException">
        /// If transaction already started.
        /// </exception>
        /// <returns></returns>
        protected IDbTransaction StartTransaction(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            IDbConnection transactionConnection = GetConnection();
            transactionConnection.Open();
            IDbTransaction transactionObject = transactionConnection.BeginTransaction(level);
            return transactionObject;
        }

        /// <summary>
        /// Get the transaction object.
        /// </summary>
        protected IDbTransaction Transaction { get; set; }

        /// <summary>
        /// Get boolean value that indicates if SQL operations should run as a database transaction.
        /// </summary>
        protected bool RunAsTransaction { get; set; }

        #endregion // Properties 

        #region Methods

        /// <summary>
        /// Commit transaction.
        /// </summary>
        /// <param name="tran"></param>
        protected void CommitTransaction(IDbTransaction tran)
        {
            EndTransaction(tran, true); // true = commit
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        /// <param name="tran"></param>
        protected void RollbackTransaction(IDbTransaction tran)
        {
            EndTransaction(tran, false);  // false = rollback
        }

        /// <summary>
        /// Helper method that ends transaction.  If "commit" parameter is
        /// true, the transaction will be committed.  If not, the transaction
        /// will be rolled back.
        /// </summary>
        /// <param name="transactionObject"></param>
        /// <param name="commit">
        /// True to commit, false to rollback.
        /// </param>
        protected void EndTransaction(IDbTransaction transactionObject, bool commit)
        {
            // Check if transaction has been started. If not, do nothing.
            // The commit or rollback operation could error out
            // if transaction already committed or rolled back
            if (transactionObject?.Connection != null)
            {
                IDbConnection connectionToTerminate = transactionObject.Connection;
                try
                {
                    if (commit)
                    {
                        transactionObject.Commit();
                    }
                    else
                    {
                        transactionObject.Rollback();
                    }
                }
                finally
                {
                    TerminateConnection(connectionToTerminate);  // close connection
                    transactionObject = null;
                    connectionToTerminate = null;
                }
            }
        }

        /// <summary>
        /// Terminate database connection.
        /// </summary>
        /// <param name="connection"></param>
        protected void TerminateConnection(IDbConnection connection)
        {
            if (connection != null)
            {
                try
                {
                    connection.Close();
                    connection.Dispose();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Returns the reference to the first table in the given dataset, or null if
        /// dataset is null or empty.
        /// </summary>
        /// <param name="dataSet">Dataset</param>
        /// <returns>A reference to first table or null</returns>
        protected DataTable GetFirstTable(DataSet dataSet)
        {
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable firstTable = dataSet.Tables[0];
                return firstTable;
            }
            return null;
        }

        /// <summary>
        /// Get fist row out of the first table in the the given dataset.
        /// </summary>
        /// <param name="dataSet">A reference to DataSet object from which to return first row</param>
        /// <returns>The first row of the first table in the given dataset, or null if dataset null or empty</returns>
        protected DataRow GetFirstRow(DataSet dataSet)
        {
            DataTable tbl = GetFirstTable(dataSet);
            if (tbl?.Rows.Count > 0)
            {
                return tbl.Rows[0]; // first row only
            }
            return null;
        }

        /// <summary>
        /// Prepare specified command for execution.  This method will check
        /// if the command is valid (not null and has command text set) and 
        /// then it will assign transaction object, connection, timeout value
        /// and prepare the command.
        /// </summary>
        /// <param name="cmd">Command to prepare</param>
        /// <param name="parms"></param>
        /// <returns>Prepared command</returns>
        protected IDbCommand PrepCommand(IDbCommand cmd, IDbDataParameter[] parms)
        {
            CheckCommand(cmd);
            if (RunAsTransaction)
            {
                if (Transaction == null)
                {
                    throw new InvalidOperationException("Transaction not started");
                }

                cmd.Transaction = Transaction;
                cmd.Connection = cmd.Transaction.Connection;
            }
            else
            {
                IDbConnection dbConnection = GetConnection();
                cmd.Connection = dbConnection;
            }

            AddParmsToCommand(cmd, parms);
            cmd.CommandTimeout = CommandTimeout;
            cmd.Prepare();

            return cmd;
        }
        protected void AddParmsToCommand(IDbCommand cmd, IDbDataParameter[] parms)
        {
            if (parms == null) return;
            foreach (IDbDataParameter p in parms)
            {
                cmd.Parameters.Add(p);
            }
        }

        private void CheckCommand(IDbCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentException("Command cannot be null."); /* error out if null */
            }
            if (string.IsNullOrEmpty(cmd.CommandText))
            {
                throw new ArgumentException("Command text has not been specified.");
            }
        }

        private void CheckTransaction(IDbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new NullReferenceException("Transaction object not specified.");
            }
            if (transaction.Connection == null)
            {
                throw new NullReferenceException("Connection object associated with the transaction is not specified.");
            }
        }

        #endregion // Methods
    }
}
