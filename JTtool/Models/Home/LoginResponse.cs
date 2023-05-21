using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace JTtool.Models.Home
{
    public class LoginResponse : BaseResponse<LoginResponseData>
    {
    }
    public class LoginResponseData
    {
        public string Redirect { get; set; }
        public short AId { get; set; }
    }
}