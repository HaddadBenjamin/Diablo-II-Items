﻿using System;
using System.IO;
using DiabloII.Application.Filters.ErrorHandling;
using DiabloII.Domain.Configurations;
using DiabloII.Infrastructure.DbContext;
using DiabloII.Infrastructure.Helpers;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace DiabloII.Application.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddMySwagger(this IServiceCollection services) => services
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Diablo II - Enriched API",
                    Description = "Allow to search Diablo II items, create suggestions, vote and comment them to drive our development, to create and search users, notify our users by email and in the application when something happened.",
                    Contact = new OpenApiContact
                    {
                        Name = "Un passionné dans la foule (alias Firefouks)",
                        Url = new Uri("https://github.com/HaddadBenjamin")
                    }
                });

                options.DescribeAllEnumsAsStrings();

                var assemblyDirectory = AppContext.BaseDirectory;
                var swaggerConfigurationFilePath = Path.Combine(assemblyDirectory, "Swagger.xml");

                options.IncludeXmlComments(swaggerConfigurationFilePath);
            });

        public static IServiceCollection AddMyMvc(this IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add(new ErrorHandlingFilter());
                })
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddFluentValidation();

            return services;
        }

        public static IServiceCollection AddMySmtpServer(this IServiceCollection services, SmtpConfiguration smtpConfiguration)
        {
            if (smtpConfiguration.EnableService)
                services
                    .AddFluentEmail(smtpConfiguration.FromEmail)
                    .AddSmtpSender(smtpConfiguration.Host, smtpConfiguration.Port, smtpConfiguration.FromEmail, smtpConfiguration.FromPassword);

            return services;
        }

        public static IServiceCollection RegisterTheDbContextDependency(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = DatabaseHelpers.GetMyConnectionString(configuration);

            return services
                .AddDbContextPool<ApplicationDbContext>(optionsBuilder =>
                    optionsBuilder.UseSqlServer(connectionString,
                        sqlServerOptions =>
                        {
                            sqlServerOptions.MigrationsAssembly("DiabloII.Application");
                            sqlServerOptions.EnableRetryOnFailure();
                        }));
        }
    }
}