﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWheels.Modules.Auth.UI
{
    public interface IUserSignInRequest
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}
