using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDataKatmanı.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email alanı gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı alanı gereklidir.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalıdır.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
