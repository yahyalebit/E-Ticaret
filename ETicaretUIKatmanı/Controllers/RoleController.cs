using System.Threading.Tasks;
using ETicaretDataKatmanı.Identity;
using ETicaretDataKatmanı.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretUIKatmanı.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            if (_roleManager.Roles.ToList() == null)
            {
                return NotFound("Roller Bulunamadı");
            }
            return View(_roleManager.Roles.Where(i => i.Name != "Admin").ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            var _role = await _roleManager.FindByNameAsync(role.Name);
            if (_role == null)
            {
                var sonuc = await _roleManager.CreateAsync(new AppRole(role.Name));
                if (sonuc.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AppRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            role.Name = model.Name;
            role.NormalizedName = model.Name.ToUpper();
            var sonuc = await _roleManager.UpdateAsync(role);
            if (sonuc.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString().ToLower());
            var sonuc = await _roleManager.DeleteAsync(role); 
            if (sonuc.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
                return View();
        }
        
    }
}
