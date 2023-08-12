using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatformService.Models
{
	[Table("Platform")] // Make sure to specify the correct table name
	public class Platform
	{
		[Key]
		[Required]
		public int Id { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public string? publisher { get; set; }
		[Required]
		public string? Cost { get; set; }

		public Platform()
		{
		}
	}
}

