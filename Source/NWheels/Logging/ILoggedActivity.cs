﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWheels.Logging
{
    public interface ILoggedActivity : IDisposable
    {
        void Warn(Exception error);
        void Fail(Exception error);
    }
}
