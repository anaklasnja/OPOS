using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.data
{
    interface IDatabaseInfo
    {
        /// <summary>
        /// Returns connection info for the database
        /// </summary>
        string ConnectionString { get; }
    }
}
