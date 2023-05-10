﻿using Connectio.Models;

namespace Connectio.Data
{
    public interface IUserRepository
    {
        ApplicationUser? GetUserByUserName(string username);
        void UpdateFollower(ApplicationUser following);
    }
}
