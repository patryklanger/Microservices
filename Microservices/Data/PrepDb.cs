﻿using Microservices.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
               SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
            if (context.Platforms.Any())
            {
                Console.WriteLine("--> We already do have data");
                return;
            }
            Console.WriteLine("--> Seeding data");

            context.Platforms.AddRange(
                new Platform() { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );

            context.SaveChanges();
        }
    }
}
