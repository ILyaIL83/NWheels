﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWheels.Concurrency
{
    public enum ResourceLockMode
    {
        Exclusive,
        MultipleReadrersExclusiveSingleWriter,
        MultipleReadrersConcurrentSingleWriter
    }
}
