using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeShoppe.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Mời nhập user name")]
        public string UserName { set; get; }
        [Required(ErrorMessage = "Mời nhập pass word")]
        public string PassWord { set; get; }
        [Required(ErrorMessage = "Ghi nhớ mật khẩu ???")]
        public bool RememberMe { set; get; }
    }
}