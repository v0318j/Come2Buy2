using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTtool.Models.Home
{
    public class LoginResponse : BaseResponse
    {
        public string Redirect { get; set; }
    }
}