using System.Drawing.Text;
using ETicaretDalKatmanı.Abstract;
using ETicaretDalKatmanı.Concrete;
using ETicaretDataKatmanı.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETicaretUIKatmanı.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryDal _categoryDal;
        public CategoryController(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public async Task<IActionResult> Index()
        {
            var product = _categoryDal.GetAll();
            return View(product);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var category = _categoryDal.GetById(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // Kategoriyi veritabanına ekleyin
                _categoryDal.Create(category);

                // Başarılı işlemin ardından kullanıcıyı kategori listesine yönlendirin
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var category = _categoryDal.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Description,CategoryName")] Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var existingCategory = _categoryDal.GetById(id);
                if (existingCategory == null)
                {
                    return NotFound();
                }
                return View(existingCategory);
            }

            try
            {
                var existingCategory = _categoryDal.GetById(id);
                if (existingCategory == null)
                {
                    return NotFound();
                }

                // Sadece güncellenebilir alanları değiştir
                existingCategory.Description = category.Description;
                existingCategory.CategoryName = category.CategoryName;

                _categoryDal.Update(existingCategory);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the category. Please try again.");
                return View(category);
            }
        }
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var category = _categoryDal.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category); // Onay için Delete View'ine yönlendir
        }

        // Kullanıcı Onay Verirse Ürünü Sil (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryDal.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            _categoryDal.Delete(category);

            return RedirectToAction("Index"); // Silindikten sonra listeye yönlendir
        }
    }
}
