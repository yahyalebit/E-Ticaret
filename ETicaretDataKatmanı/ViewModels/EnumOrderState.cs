﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretDataKatmanı.ViewModels
{
    public enum EnumOrderState
    {
        [Display(Name ="Onay Bekliyor..")]
        Waiting,
        [Display(Name ="Tamamlandı")]
        Completed
    }
}
