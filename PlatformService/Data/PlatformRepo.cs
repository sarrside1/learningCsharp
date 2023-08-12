using System;
using PlatformService.Models;

namespace PlatformService.Data
{
	public class PlatformRepo: IPlatformRepo
	{
        private readonly AppDbContext _dbContext;
		public PlatformRepo(AppDbContext dbContext)
		{
            _dbContext = dbContext;
		}

        void IPlatformRepo.CreatePlatform(Platform platform)
        {
            if (platform != null)
            {
                _dbContext.Add(platform);
            }
            else
                throw new ArgumentNullException(nameof(Platform));
        }

        IEnumerable<Platform> IPlatformRepo.GetAllPlatform()
        {
            return _dbContext.Platforms!.ToList();
        }

        Platform IPlatformRepo.GetPlatformById(int id)
        {
            return _dbContext.Platforms.FirstOrDefault(p => p.Id == id);
        }

        bool IPlatformRepo.SaveChanges()
        {
            return (_dbContext.SaveChanges()>=0);
        }
    }
}

