﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWheels.UI.Elements;

namespace NWheels.UI
{
    public interface IUiApplication
    {
        void BuildApplication(IUiApplicationBuilder builder);
    }
}
