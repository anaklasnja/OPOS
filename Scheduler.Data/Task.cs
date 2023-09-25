using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.data
{
    public class Task
    {
        public int Id { get; set; }
        public string MediaFile { get; set; }
        public string AlogorithmCode { get; set; }
        public int TaskTypeId { get; set; }
        public string RequestedStart { get; set; }
        public int MaxExecutionSeconds { get; set; }
        public string LatestCompletion { get; set; }
    }
}
