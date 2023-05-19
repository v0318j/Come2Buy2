using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTtool.Models.Home
{
    public class AccountResponse : BaseResponse
    {
        public short Id { get; set; }
        public string LoginId { get; set; }
        public string Name { get; set; }
    }
}