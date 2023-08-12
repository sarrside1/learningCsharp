using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using PlatformService.Models;

namespace PlatformService.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app, bool isProd)
		{
			using(var serviceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!, isProd);
			}
		}

		private static void SeedData(AppDbContext dbContext, bool isProd)
		{
			if(isProd)
			{
				Console.WriteLine("--> Migrating database.....");
				try
				{
					dbContext.Database.Migrate();
				}
				catch (Exception ex)
				{
					
					Console.WriteLine($"--> Could not migrate database {ex.Message}");
				}
			}
			if (!dbContext.Platforms.Any())
			{
				Console.WriteLine("--> Seeding Data .....");
				dbContext.AddRange(
					new Platform() { Name="Dot Net", publisher="Microsoft", Cost="Free"},
                    new Platform() { Name = "Sql Server Express", publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernates", publisher = "Cloud Native Computing Foundation", Cost = "Free" }

                );
				dbContext.SaveChanges();
			}
			else
			{
				Console.WriteLine("--> We already have data");
			}
		}
	}
}

