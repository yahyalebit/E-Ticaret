using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretDataKatmanı.Entities;

namespace ETicaretDataKatmanı.ViewModels
{
    public class ListViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public ListViewModel()
        {
            Products = new List<Product>();
        }
    }
}
