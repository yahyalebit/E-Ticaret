using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretDataKatmanı.ViewModels;

namespace ETicaretDataKatmanı.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
		public string OrderNumber { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public EnumOrderState OrderState { get; set; }
		public string UserName { get; set; }
		public string AddressTitle { get; set; }
		public string Address { get; set; }
        public bool IsApproved { get; set; }
        public virtual List<OrderLine> OrderLines { get; set; }
		// virtual anahtar kelimesi ile ilişkili nesnelerin yüklenmesi ertelenir. yani lazy loading yapılır.
		// Lazy loading ile ilişkili nesneler sadece ihtiyaç duyulduğunda yüklenir. 
		// Bir siparişin birden fazla sipariş satırı(yani detayı) olabilir.
	}
}
