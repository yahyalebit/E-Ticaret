using ETicaretBusinessKatmanı.Abstract;
using ETicaretDataKatmanı.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDalKatmanı.Abstract
{
    public interface IOrderLineDal : IGenericRepository<OrderLine>
	{
    }
}
