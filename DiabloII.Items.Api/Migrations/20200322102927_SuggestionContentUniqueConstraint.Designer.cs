﻿// <auto-generated />
using System;
using DiabloII.Items.Api.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DiabloII.Items.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200322102927_SuggestionContentUniqueConstraint")]
    partial class SuggestionContentUniqueConstraint
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DiabloII.Items.Api.DbContext.Suggestions.Suggestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Content")
                        .IsUnique();

                    b.ToTable("Suggestions");
                });

            modelBuilder.Entity("DiabloII.Items.Api.DbContext.Suggestions.SuggestionVote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasMaxLength(15);

                    b.Property<bool>("IsPositive")
                        .HasColumnType("bit");

                    b.Property<Guid>("SuggestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SuggestionId");

                    b.ToTable("SuggestionVotes");
                });

            modelBuilder.Entity("DiabloII.Items.Api.DbContext.Suggestions.SuggestionVote", b =>
                {
                    b.HasOne("DiabloII.Items.Api.DbContext.Suggestions.Suggestion", "Suggestion")
                        .WithMany("Votes")
                        .HasForeignKey("SuggestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
