using System.Diagnostics;
using ETicaretDataKatmanı.Identity;
using ETicaretDataKatmanı.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaretUIKatmanı.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Admin rolündeki kullanıcıları çek
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            // Admin kullanıcılarının ID'lerini al
            var Id = admins.Select(a => a.Id).ToList();

            // Admin ID'lerini kontrol etmek için log ekleyin (isteğe bağlı)
            Console.WriteLine($"Admin IDs: {string.Join(", ", Id)}");

            // Admin olmayan kullanıcıları filtrele
            var users = await _userManager.Users
                .Where(i => !Id.Contains(i.Id))  // Admin olmayanları filtrele
                .ToListAsync();

            return View(users);
        }
        // GET: /User/RoleAssign/{id}
        [HttpGet]
        public async Task<IActionResult> RoleAssign(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = _roleManager.Roles.Where(i => i.Name != "Admin").ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var RoleAssigns = new List<RoleAssignModel>();
            roles.ForEach(role => RoleAssigns.Add(new RoleAssignModel
            {
                HasAssign = userRoles.Contains(role.Name),
                Id = role.Id,
                Name = role.Name
            }));
            ViewBag.name = user.Name;
            return View(RoleAssigns);
        }
        [HttpPost, ActionName("RoleAssign")]
        public async Task<IActionResult> RoleAssign(List<RoleAssignModel> models, int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Index");
            }

            // Eğer SecurityStamp null ise, yeni bir SecurityStamp oluştur
            if (string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);
            }

            // Kullanıcının mevcut rollerini al
            var userRoles = await _userManager.GetRolesAsync(user);

            // Kullanıcının tüm rollerini kaldır
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                TempData["ErrorMessage"] = "Roller kaldırılırken bir hata oluştu.";
                return RedirectToAction("Index");
            }

            // Seçili olan rolleri ekle
            var selectedRoles = models.Where(x => x.HasAssign).Select(x => x.Name).ToList();
            var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);
            if (!addResult.Succeeded)
            {
                TempData["ErrorMessage"] = "Yeni roller eklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Kullanıcının rolleri başarıyla güncellendi!";
            return RedirectToAction("Index");
        }

        



    }
}
