﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.data
{
    public class Alogorithm
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
