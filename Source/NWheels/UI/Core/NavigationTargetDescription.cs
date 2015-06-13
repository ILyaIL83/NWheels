﻿using System;

namespace NWheels.UI.Core
{
    public abstract class NavigationTargetDescription : UIElementContainerDescription
    {
        public WidgetDescription ContentRoot { get; set; }
        public Type InputParameterType { get; set; }
    }
}
