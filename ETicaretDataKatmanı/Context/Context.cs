using ETicaretDataKatmanı.Entities;
using ETicaretDataKatmanı.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDataKatmanı.Context
{
	public class ETicaretContext : IdentityDbContext<AppUser, AppRole, int>
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-TLASFF1\\SQLEXPRESS;Database=eTicaretCore;Trusted_Connection=True;");
		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<OrderLine> OrderLines { get; set; }
	}
}
