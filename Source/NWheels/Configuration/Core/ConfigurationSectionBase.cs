﻿using NWheels.Conventions.Core;

namespace NWheels.Configuration.Core
{
    public abstract class ConfigurationSectionBase : ConfigurationElementBase, IConfigurationSection
    {
        protected ConfigurationSectionBase(IConfigurationObjectFactory factory, Auto<IConfigurationLogger> logger, string configPath)
            : base(factory, logger, configPath)
        {
        }
    }
}
