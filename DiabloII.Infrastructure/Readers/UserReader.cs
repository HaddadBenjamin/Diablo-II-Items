﻿using System.Collections.Generic;
using DiabloII.Domain.Models.Users;
using DiabloII.Domain.Readers;
using DiabloII.Domain.Repositories;

namespace DiabloII.Infrastructure.Readers
{
    public class UserReader : IUserReader
    {
        private readonly IUserRepository _repository;

        public UserReader(IUserRepository repository) => _repository = repository;

        public IReadOnlyCollection<User> GetAll() => _repository.GetAll();
    }
}