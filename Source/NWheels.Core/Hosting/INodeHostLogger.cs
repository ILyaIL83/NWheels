﻿using System;
using NWheels.Exceptions;
using NWheels.Logging;

namespace NWheels.Core.Hosting
{
    public interface INodeHostLogger : IApplicationEventLogger
    {
        [LogThread(ThreadTaskType.StartUp)]
        ILogActivity NodeStartingUp();

        [LogThread(ThreadTaskType.StartUp)]
        ILogActivity NodeLoading();

        [LogActivity]
        ILogActivity LoadingModules();

        [LogCritical]
        void FailedToLoadModule(string moduleName, string moduleClass, Exception e);

        [LogVerbose]
        void RegisteringModule(string module);

        [LogWarning]
        void NoApplicationModulesRegistered();

        [LogActivity]
        ILogActivity LookingForLifecycleComponents();

        [LogVerbose]
        void FoundLifecycleComponent(string component);

        [LogWarning]
        void NoLifecycleComponentsFound();

        [LogCritical]
        void FailedToLoadLifecycleComponents(Exception e);

        [LogActivity]
        ILogActivity ComponentNodeLoading(string component);

        [LogActivity]
        ILogActivity ComponentLoading(string component);

        [LogActivity]
        ILogActivity ComponentNodeLoaded(string component);

        [LogInfo]
        void NodeSuccessfullyLoaded();

        [LogError]
        NodeHostException NodeHasFailedToLoad();

        [LogError]
        void NodeLoadError(Exception e);

        [LogThread(ThreadTaskType.StartUp)]
        ILogActivity NodeActivating();

        [LogActivity]
        ILogActivity ComponentNodeActivating(string component);

        [LogActivity]
        ILogActivity ComponentActivating(string component);

        [LogActivity]
        ILogActivity ComponentNodeActivated(string component);

        [LogInfo]
        void NodeSuccessfullyActivated();

        [LogError]
        void NodeActivationError(Exception e);

        [LogError]
        NodeHostException NodeHasFailedToActivate();

        [LogError]
        void NodeHasFailedToActivate(Exception e);

        [LogThread(ThreadTaskType.ShutDown)]
        ILogActivity NodeShuttingDown();

        [LogThread(ThreadTaskType.ShutDown)]
        ILogActivity NodeDeactivating();

        [LogActivity]
        ILogActivity ComponentNodeDeactivating(string component);

        [LogActivity]
        ILogActivity ComponentDeactivating(string component);

        [LogActivity]
        ILogActivity ComponentNodeDeactivated(string component);

        [LogError]
        void NodeDeactivationError(Exception e);

        [LogError]
        NodeHostException NodeHasFailedToDeactivate();

        [LogInfo]
        void NodeDeactivated();

        [LogThread(ThreadTaskType.ShutDown)]
        ILogActivity NodeUnloading();

        [LogActivity]
        ILogActivity ComponentNodeUnloading(string component);

        [LogActivity]
        ILogActivity ComponentUnloading(string component);

        [LogActivity]
        ILogActivity ComponentNodeUnloaded(string component);

        [LogInfo]
        void NodeUnloaded();

        [LogError]
        void NodeUnloadError(Exception e);

        [LogError]
        NodeHostException NodeHasFailedToUnload();
    }
}