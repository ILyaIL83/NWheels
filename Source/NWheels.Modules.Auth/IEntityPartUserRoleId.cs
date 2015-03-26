﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWheels.Entities;

namespace NWheels.Modules.Auth
{
    [EntityPartContract]
    public interface IEntityPartUserRoleId<TRoleId>
    {
        TRoleId RoleId { get; set; }
    }
}
