﻿using NWheels.Compilation.Mechanism.Syntax.Members;
using System;
using System.Collections.Generic;
using System.Text;

namespace NWheels.Compilation.Mechanism.Factories
{
    public interface ITypeFactoryContext
    {
        TypeKey Key { get; }
        object Extension { get; }
        TypeMember Type { get; }
    }

    public interface ITypeFactoryContext<TExtension>
    {
        TypeKey Key { get; }
        TExtension Extension { get; }
        TypeMember Type { get; }
    }
}
