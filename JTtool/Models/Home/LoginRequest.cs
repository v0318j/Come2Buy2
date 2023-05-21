using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JTtool.Models.Home
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="請輸入帳號")]
        public string LoginId { get; set; }
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
}