﻿using System.Collections.Generic;
using System.Linq;
using DiabloII.Items.Api.DbContext;
using DiabloII.Items.Api.DbContext.ErrorLogs.Models;

namespace DiabloII.Items.Api.Services.ErrorLogs
{
    public class ErrorLogsService : IErrorLogsService
    {
        private readonly ApplicationDbContext _dbContext;

        public ErrorLogsService(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public void Log(ErrorLog errorLog)
        {
            _dbContext.ErrorLogs.Add(errorLog);
            _dbContext.SaveChanges();
        }

        public IReadOnlyCollection<ErrorLog> GetAll() => _dbContext.ErrorLogs
            .OrderBy(errorLog => errorLog.CreationDateUtc)
            .ToList();
    }
}