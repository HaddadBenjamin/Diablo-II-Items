﻿using System.Collections.Generic;
using DiabloII.Domain.Models.Users;

namespace DiabloII.Domain.Repositories
{
    public interface IUserRepository
    {
        bool DoesUserExists(string userId);

        User GetUser(string user);

        IEnumerable<User> GetAllUsers();

        IEnumerable<User> GetUsers(IReadOnlyCollection<string> userIds);
    }
}