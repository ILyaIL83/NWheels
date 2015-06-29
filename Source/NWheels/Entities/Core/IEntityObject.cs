﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWheels.Entities.Core
{
    public interface IEntityObject
    {
        IEntityId GetId();
        Type Contract { get; }
    }
}
