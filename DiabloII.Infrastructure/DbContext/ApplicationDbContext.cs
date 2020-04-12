﻿using DiabloII.Domain.Models.ErrorLogs;
using DiabloII.Domain.Models.Items;
using DiabloII.Domain.Models.Suggestions;
using DiabloII.Infrastructure.DbContext.Mappers.Items;
using DiabloII.Infrastructure.DbContext.Mappers.Suggestions;
using Microsoft.EntityFrameworkCore;

namespace DiabloII.Infrastructure.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Suggestion> Suggestions { get; set; }

        public DbSet<SuggestionVote> SuggestionVotes { get; set; }

        public DbSet<SuggestionComment> SuggestionComments { get; set; }

        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public DbSet<Item> Items{ get; set; }
        
        public DbSet<ItemProperty> ItemProperties{ get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SuggestionDbMapper.Map(modelBuilder);
            SuggestionVoteDbMapper.Map(modelBuilder);
            SuggestionCommentDbMapper.Map(modelBuilder);

            ItemMapper.Map(modelBuilder);
            ItemPropertyMapper.Map(modelBuilder);
        }
    }
}