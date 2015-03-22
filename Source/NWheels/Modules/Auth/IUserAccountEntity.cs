﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWheels.Entities;

namespace NWheels.Modules.Auth
{
    [EntityContract]
    public interface IUserAccountEntity
    {
        string LoginName { get; set; }
        string NickName { get; set; }
        string FullName { get; set; }
        string EmailAddress { get; set; }
        bool IsEmailVerified { get; set; }
        IPasswordEntity CurrentPassword { get; set; }
        ICollection<IPasswordEntity> PasswordHistory { get; }
        ICollection<IUserRoleEntity> Roles { get; }
        DateTime LastLoginAt { get; set; }
        int FailedLoginCount { get; set; }
        bool IsLockedOut { get; set; }
    }
}
