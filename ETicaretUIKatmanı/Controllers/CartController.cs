using ETicaretDalKatmanı.Abstract;
using ETicaretDataKatmanı.Entities;
using ETicaretDataKatmanı.Helpers;
using ETicaretDataKatmanı.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ETicaretUIKatmanı.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderDal _orderDal;
        private readonly IProductDal _productDal;
        public CartController(IOrderDal orderDal, IProductDal productDal)
        {
            _orderDal = orderDal;
            _productDal = productDal;
        }
        
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart == null)
            {
                return View();
            }
            ViewBag.Total = cart.Sum(x => x.Product.Price * x.Quantity).ToString("c");
            SessionHelper.Count = cart.Count;
            return View(cart);
        }


        //public IActionResult Buy(int id)
        //{
        //    if (SessionHelper.GetObjectFromJson<List<CartItem>>
        //        (HttpContext.Session, "Cart") == null)
        //    {
        //        var cart = new List<CartItem>();
        //        cart.Add(new CartItem
        //        {
        //            Product = _productDal.GetById(id),
        //            Quantity = 1
        //        });
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
        //    }
        //    else
        //    {
        //        var cart = SessionHelper.GetObjectFromJson<List<CartItem>>
        //            (HttpContext.Session, "Cart");
        //        int index = isExist(cart, id);
        //        if (index < 0)
        //        {
        //            cart.Add(new CartItem
        //            {
        //                Product = _productDal.GetById(id),
        //                Quantity = 1
        //            });
        //        }
        //        else
        //        {
        //            cart[index].Quantity++;
        //        }
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
        //    }
        //    return RedirectToAction("Index");
        //}
        public IActionResult Buy(int id)
        {
            var product = _productDal.GetById(id);
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart") == null)
            {
                var cart = new List<CartItem>
        {
            new CartItem
            {
                Product = product,
                Quantity = 1
            }
        };
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
                int index = isExist(cart, id);

                // Eğer ürün sepette yoksa ekle
                if (index < 0)
                {
                    cart.Add(new CartItem
                    {
                        Product = product,
                        Quantity = 1
                    });
                }
                else
                {
                    // Sepetteki miktar stok miktarından fazla olamaz
                    if (cart[index].Quantity < product.Stock)
                    {
                        cart[index].Quantity++;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Bu ürün için stok sınırına ulaşıldı!";
                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
            }
            return RedirectToAction("Index");
        }



        private int isExist(List<CartItem> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        [HttpGet]
        public IActionResult CheckOut()
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (Cart == null || Cart.Count<1)
            {
                ModelState.AddModelError("404", "Sepetinizde ürün bulunmamaktadır.");
                return RedirectToAction("Index", "Cart");
            }
            return View(new ShippingDetails());
        }
        [HttpPost]
        public IActionResult CheckOut(ShippingDetails shippingDetails)
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (Cart == null)
            {
                ModelState.AddModelError("404","Sepetinizde ürün bulunmamaktadır.");
            }
            if (ModelState.IsValid)
            {
                SaveOrder(Cart, shippingDetails);
                Cart.Clear();
                SessionHelper.SetObjectAsJson(HttpContext.Session,"Cart", Cart);
            }
            return RedirectToAction("Index");
        }


        public IActionResult IncreaseQuantity(int id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart != null)
            {
                int index = isExist(cart, id);
                if (index >= 0)
                {
                    var product = _productDal.GetById(id);

                    // Eğer mevcut miktar stok miktarına ulaştıysa artırma
                    if (cart[index].Quantity < product.Stock)
                    {
                        cart[index].Quantity++;
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Stok sınırına ulaşıldı!";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(int id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart != null)
            {
                int index = isExist(cart, id);
                if (index >= 0)
                {
                    if (cart[index].Quantity > 1)
                    {
                        cart[index].Quantity--;
                    }
                    else
                    {
                        cart.RemoveAt(index);
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove (int id)
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "Cart");
            int index = isExist(Cart, id);
            Cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", Cart);
            return RedirectToAction("Index");
        }

        //private void SaveOrder(List<CartItem>? cart, ShippingDetails shippingDetails)
        //{
        //    var quint = Guid.NewGuid().ToString("N");
        //    var order = new Order();
        //    order.OrderNumber = quint;
        //    order.TotalAmount = cart.Sum(x => x.Product.Price * x.Quantity);
        //    order.OrderDate = DateTime.Now;
        //    order.OrderState = EnumOrderState.Waiting;
        //    order.UserName = shippingDetails.UserName;
        //    order.Address = shippingDetails.Address;
        //    order.AddressTitle= shippingDetails.AdressTitle;

        //    order.OrderLines = new List<OrderLine>();

        //    foreach (var item in cart)
        //    {
        //        var orderLine = new OrderLine();
        //        orderLine.OrderLineQuantity = item.Quantity;
        //        orderLine.OrderLinePrice = item.Quantity * item.Product.Price;
        //        orderLine.ProductId = item.Product.ProductId;
        //        order.OrderLines.Add(orderLine);
        //    }
        //    _orderDal.Create(order);
        //}
        private void SaveOrder(List<CartItem>? cart, ShippingDetails shippingDetails)
        {
            var orderNumber = Guid.NewGuid().ToString("N");
            var order = new Order
            {
                OrderNumber = orderNumber,
                TotalAmount = cart.Sum(x => x.Product.Price * x.Quantity),
                OrderDate = DateTime.Now,
                OrderState = EnumOrderState.Waiting,
                UserName = shippingDetails.UserName,
                Address = shippingDetails.Address,
                AddressTitle = shippingDetails.AdressTitle,
                OrderLines = new List<OrderLine>()
            };

            foreach (var item in cart)
            {
                var product = _productDal.GetById(item.Product.ProductId);

                // Eğer yeterli stok yoksa siparişi iptal et
                if (product.Stock < item.Quantity)
                {
                    throw new Exception($"Ürün '{product.Name}' için yeterli stok bulunmamaktadır!");
                }

                // Stoktan düş
                product.Stock -= item.Quantity;

                // Eğer stok 0 olursa IsApprove ve IsHome false olsun
                if (product.Stock == 0)
                {
                    product.IsApproved = false;
                    product.IsHome = false;
                }

                _productDal.Update(product); // Güncellenmiş ürünü veritabanına kaydet

                var orderLine = new OrderLine
                {
                    OrderLineQuantity = item.Quantity,
                    OrderLinePrice = item.Quantity * item.Product.Price,
                    ProductId = item.Product.ProductId
                };
                order.OrderLines.Add(orderLine);
            }

            _orderDal.Create(order);
        }



    }
}
