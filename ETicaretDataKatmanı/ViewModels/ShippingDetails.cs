using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDataKatmanı.ViewModels
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="Lütfen İsminizi Giriniz...")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Lütfen Adres Başlığını Giriniz...")]
        public string AdressTitle { get; set; }
        [Required(ErrorMessage = "Lütfen Adres Giriniz...")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Lütfen Şehir Giriniz...")]
        public string City { get; set; }
        //[Required(ErrorMessage = "Lütfen Email Giriniz...")]
        //public string Email { get; set; }

    }
}
