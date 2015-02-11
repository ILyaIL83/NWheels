﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWheels.Modules.Crm
{
    public interface ICustomerRelationshipEntity
    {
        ICustomerEntity Relating { get; set; }
        ICustomerEntity Related { get; set; }
        ICustomerRelationshipTypeEntity Type { get; set; }
    }
}
