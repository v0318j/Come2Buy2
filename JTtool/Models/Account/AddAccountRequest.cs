﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JTtool.Models.Home
{
    public class AddAccountRequest
    {
        [Required(ErrorMessage = "請輸入帳號密碼")]
        public string LoginId { get; set; }
        [Required(ErrorMessage = "請輸入帳號密碼")]
        public string Password { get; set; }
        [Required(ErrorMessage = "請輸入帳號密碼")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "請輸入姓名")]
        public string Name { get; set; }
    }
}