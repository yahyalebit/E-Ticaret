using ETicaretBusinessKatmanı.Concrete;
using ETicaretDalKatmanı.Abstract;
using ETicaretDataKatmanı.Context;
using ETicaretDataKatmanı.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDalKatmanı.Concrete
{
    public class OrderDal : GenericRepository<Order, ETicaretContext>, IOrderDal
	{
    }
}
