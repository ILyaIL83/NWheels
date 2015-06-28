﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NWheels.Conventions;
using NWheels.Entities;
using NWheels.Extensions;
using NWheels.Hosting;
using NWheels.Stacks.MongoDb.Impl;

namespace NWheels.Stacks.MongoDb
{
    public class ModuleLoader : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MongoDataRepositoryFactory>().As<IDataRepositoryFactory, IAutoObjectFactory>().SingleInstance();
            //TODO: add logging component
        }
    }
}
