using ETicaretDalKatmanı.Abstract;
using ETicaretDataKatmanı.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretUIKatmanı.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
       
        private readonly IOrderDal _orderDal;
        public OrderController(IOrderDal order)
        {
            _orderDal = order;
        }
        public IActionResult Index()
        {
            var sonuc = _orderDal.GetAll();
            return View(sonuc);
        }
        public IActionResult ToggleApproval(int orderId)
        {
            // Siparişi bul
            var order = _orderDal.GetById(orderId);
            if (order != null)
            {
                // IsApproved değerini tersine çevir
                order.IsApproved = !order.IsApproved;
                _orderDal.Update(order);  // Veritabanındaki kaydı güncelle

                // İşlem sonrası, sipariş listesini tekrar yükle
                return RedirectToAction("New");
            }

            // Sipariş bulunamazsa hata sayfasına yönlendir
            return NotFound();
        }
        public IActionResult New()
        {
            var sonuc = _orderDal.GetAll().Where(i => i.IsApproved == false);
            return View(sonuc);
        }
        public IActionResult Approved()
        {
            var sonuc = _orderDal.GetAll().Where(i => i.IsApproved == true);
            return View(sonuc);
        }
    }
}
