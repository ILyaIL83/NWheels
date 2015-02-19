﻿using System;
using NWheels.Hosting;

namespace NWheels.Logging
{
    internal interface IThreadLog
    {
        void NotifyActivityClosed(ActivityLogNode activity);
        INodeConfiguration Node { get; }
        ThreadTaskType TaskType { get; }
        Guid LogId { get; }
        Guid CorrelationId { get; }
        DateTime ThreadStartedAtUtc { get; }
        long ElapsedThreadMilliseconds { get; }
        ActivityLogNode RootActivity { get; }
        ActivityLogNode CurrentActivity { get; }
    }
}
