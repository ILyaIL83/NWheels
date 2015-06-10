﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWheels.Extensions;
using NWheels.Hosting;
using NWheels.Processing.Workflows;
using NWheels.Processing.Workflows.Core;
using NWheels.Processing.Workflows.Impl;

namespace NWheels.Processing
{
    public class WorkflowEngineFeatureLoader : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.NWheelsFeatures().Logging().RegisterLogger<IWorkflowEngineLogger>();
            builder.NWheelsFeatures().Logging().RegisterLogger<TransientWorkflowReadyQueue.ILogger>();
            builder.NWheelsFeatures().Hosting().RegisterLifecycleComponent<TransientWorkflowReadyQueue>().As<IWorkflowReadyQueue>();
            builder.NWheelsFeatures().Hosting().RegisterLifecycleComponent<WorkflowEngine>().As<IWorkflowEngine, IWorkflowInstanceContainer>();
        }
    }
}
