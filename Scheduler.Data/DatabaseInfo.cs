using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace sample.data
{
    public abstract class DatabaseInfo : IDatabaseInfo
    {
        /// <summary>
        /// Get the connection string.
        /// </summary>
        public string ConnectionString => BuildConnectionString();

        /// <summary>
        /// Build the connection string.
        /// </summary>
        /// <returns></returns>
        protected abstract string BuildConnectionString();
    }
}
