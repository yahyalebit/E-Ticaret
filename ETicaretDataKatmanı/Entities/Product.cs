using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDataKatmanı.Entities
{
	public class Product
	{
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? Image { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public bool IsApproved { get; set; }
		public bool IsHome { get; set; }
		public int CategoryId { get; set; }
		public virtual Category? category { get; set; }

        public Product()
		{

		}


    }
}
