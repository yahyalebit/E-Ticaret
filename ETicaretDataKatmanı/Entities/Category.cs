using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDataKatmanı.Entities
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string? Description { get; set; }
		public string CategoryName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public List<Product> Products { get; set; }
		public Category()
		{
			Products = new List<Product>();
		}
	}
}
