using BulkyWeb_Razor.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb_Razor.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Category> Categories { get; set; }
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
				new Category { Id = 2, Name = "SciFi", DisplayOrder = 1 },
				new Category { Id = 3, Name = "History", DisplayOrder = 3 }
				);
		}
	}
}
