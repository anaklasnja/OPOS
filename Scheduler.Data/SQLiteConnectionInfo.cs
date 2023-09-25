using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace sample.data
{
    public class SQLiteConnectionInfo : DatabaseInfo
    {
        #region Constructors 

        public SQLiteConnectionInfo(string dbFile, int dbVersion)
        {
            File = dbFile;
            Version = dbVersion;
        }

        #endregion // Constructors 

        protected override string BuildConnectionString()
        {
            return $"Data Source={FullPathDatabaseFile};Version={Version};";
        }

        #region Properties

        public string File { get; protected set; }
        public int Version { get; protected set; }
        private string Directory
        {
            get
            {
                string exePath = Assembly.GetEntryAssembly().Location;
                return Path.GetDirectoryName(exePath);
            }
        }
        private string FullPathDatabaseFile => Path.Combine(Directory, File);

        #endregion // Properties
    }
}
