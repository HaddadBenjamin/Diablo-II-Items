﻿using DiabloII.Items.Api.DbContext.DbMappers;
using DiabloII.Items.Api.DbContext.Suggestions;
using Microsoft.EntityFrameworkCore;

namespace DiabloII.Items.Api.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Suggestion> Suggestions { get; set; }

        public DbSet<SuggestionVote> SuggestionVotes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public ApplicationDbContext() { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SuggestionDbMapper.Map(modelBuilder);
            SuggestionVoteDbMapper.Map(modelBuilder);
        }
    }
}
