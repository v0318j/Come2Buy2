using JTtool.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTtool.Models.Home
{
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string CheckPassword { get; set; }
    }
}