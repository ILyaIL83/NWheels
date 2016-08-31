using System;

namespace NWheels.Api.Concurrency
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum SyncAccessTypes
    {
        None = 0,
        Read = 0x01,
        Write = 0x02,
        ReadWrite = Read | Write
    }
}
