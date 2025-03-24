using ETicaretDalKatmanı.Abstract;
using ETicaretDataKatmanı.Context;
using ETicaretDataKatmanı.Entities;
using ETicaretDataKatmanı.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETicaretUIKatmanı.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductDal _productDal;
        private readonly ICategoryDal _categoryDal;

        public ProductController(IProductDal productDal, ICategoryDal categoryDal)
        {
            _productDal = productDal;
            _categoryDal = categoryDal;
        }


        public async Task<IActionResult> Index()
        {
            var product = _productDal.GetAll();
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(_categoryDal.GetAll(), "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProductId", "Name", "Description", "Image", "Stock", "Price", "IsApproved", "IsHome", "CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productDal.Create(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList
                (_categoryDal.GetAll(), "CategoryId", "CategoryName", product.CategoryId);
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {

            var product = _productDal.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
  
            var product = _productDal.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_categoryDal.GetAll(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,Image,Stock,IsApproved,IsHome,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _productDal.Update(product);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_categoryDal.GetAll(), "CategoryId", "CategoryName",
                product.CategoryId);
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var product = _productDal.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product); // Onay için Delete View'ine yönlendir
        }

        // Kullanıcı Onay Verirse Ürünü Sil (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productDal.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productDal.Delete(product);

            return RedirectToAction("Index"); // Silindikten sonra listeye yönlendir
        }


    }
}

