using System;
using PlatformService.Models;

namespace PlatformService.Data
{
	public interface IPlatformRepo
	{
		/// <summary>
		/// Create Platform
		/// </summary>
		/// <returns></returns>
		bool SaveChanges();
		IEnumerable<Platform> GetAllPlatform();

		Platform GetPlatformById(int id);
		void CreatePlatform(Platform platform);
	}
}

