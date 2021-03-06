﻿using EventStoreTools.Core.Interfaces;
using EventStoreTools.Core.Services;
using EventStoreTools.Infrastructure.DataBase.Contexts;
using EventStoreTools.Infrastructure.DataBase.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using EventStoreTools.Core.Containers;
using Microsoft.Extensions.Logging;
using System.IO;
using EventStoreTools.Core.Interfaces.Search;
using EventStoreTools.Core.Services.Search;
using EventStoreTools.Infrastructure.EventStore.Repositories;
using EventStoreTools.Infrastructure.EventStore.Context;
using EventStoreTools.Core.Services.Search.Factories;
using EventStoreTools.Core.Interfaces.Subscribes;
using EventStoreTools.Web.Logger;
using EventStoreTools.Core.Interfaces.Restores;
using EventStoreTools.Core.Services.Retore;
using EventStoreTools.Core.Services.Backups;
using EventStoreTools.Core.Interfaces.Backups;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EventStoreTools.Core.JWT;

namespace EventStoreTools
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true
                        };
                    });

            var connection = Configuration["ConnectionStrings:EventStoreToolsConnectionString"];

            services.AddDbContext<EventStoreToolsDBContext>(options =>
                options.UseSqlite(connection)
            );



            services.AddAutoMapper(x => x.AddProfile(new MapperProfile()));
            services.AddMvc();
            services.AddCors();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IConnectionService, ConnectionService>();
            services.AddTransient<IConnectionRepository, ConnectionRepository>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IEventStoreSearchRepositopy, EventStoreSearchRepositopy>();
            services.AddTransient<IEventStoreConnectionFactory, EventStoreConnectionFactory>();
            services.AddTransient<ILogger, ApplicationLogger>();
            services.AddTransient<ISearchStrategyFactory, SearchStrategyFactory>();
            services.AddTransient<ISubscribeRepository, SubscribeRepository>();
            services.AddTransient<ISubscribesService, SubscribesService>();
            services.AddTransient<IRestoreService, RestoreService>();
            services.AddTransient<IBackupService, BackupService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = loggerFactory.CreateLogger("ApplicationLogger");



            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<EventStoreToolsDBContext>();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCors(builder =>
            builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());

            app.UseMvc();
        }
    }
}
