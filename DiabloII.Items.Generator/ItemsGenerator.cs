﻿using DiabloII.Items.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using DiabloII.Domain.Models.Items;
using DiabloII.Infrastructure.Helpers;
using DiabloII.Infrastructure.Readers;
using DiabloII.Infrastructure.Repositories;
using DiabloII.Items.Reader.Items;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ItemCategory = DiabloII.Domain.Models.Items.ItemCategory;
using ItemQuality = DiabloII.Domain.Models.Items.ItemQuality;

namespace DiabloII.Items.Generator
{
    public static class ItemsGenerator
    {
        private static readonly int CommandTimeout = 600;

        public static void Generate(GenerationEnvironment[] environments)
        {
            var uniqueItems = new DiabloIIFilesReader().Read();

            CreateTheUniqueItemsJsonFile(uniqueItems);
            UpdateTheItemsFromDatabase(environments, uniqueItems);
        }

        private static void CreateTheUniqueItemsJsonFile(IEnumerable<ItemFromFile> uniqueItems)
        {
            var uniqueItemDestinationPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/Uniques.json");
            var uniqueItemsAsJson = JsonConvert.SerializeObject(uniqueItems, Formatting.Indented);

            File.WriteAllText(uniqueItemDestinationPath, uniqueItemsAsJson);
        }

        private static void UpdateTheItemsFromDatabase(GenerationEnvironment[] environments, IEnumerable<ItemFromFile> uniqueItems)
        {
            var mapper = GetMapper();
            var items = uniqueItems.Select(item => mapper.Map<Item>(item)).ToList();
            var itemProperties = items.SelectMany(item => item.Properties).ToList();
            var configurationFilePaths = environments.Select(environment => $"appsettings.{environment.ToString()}.json");

            foreach (var configurationFilePath in configurationFilePaths)
            {
                var configuration = ConfigurationHelpers.GetMyConfiguration(configurationFilePath);
                var connectionString = DatabaseHelpers.GetMyConnectionString(configuration, "DiabloII.Items.Generator");

                using (var dbContext = DatabaseHelpers.GetMyDbContext(connectionString))
                {
                    dbContext.Database.SetCommandTimeout(CommandTimeout);
                    dbContext.Database.Migrate();

                    var itemRepository = new ItemRepository(dbContext);

                    new ItemReader(dbContext, itemRepository).ResetTheItems(items, itemProperties);
                }
            }
        }

        private static IMapper GetMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reader.Items.ItemFromFile, Item>()
                    .AfterMap((source, destination) =>
                    {
                        destination.Id = Guid.NewGuid();
                        destination.Quality = Enum.Parse<ItemQuality>(source.Quality);
                        destination.Category = Enum.Parse<ItemCategory>(source.Category);

                        destination.Properties = destination.Properties.Select(_ =>
                        {
                            _.ItemId = destination.Id;

                            return _;
                        }).ToList();
                    });
                cfg.CreateMap<Reader.Items.ItemPropertyFromFile, ItemProperty>()
                    .AfterMap((source, destination) => destination.Id = Guid.NewGuid());
            });
            var mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}



