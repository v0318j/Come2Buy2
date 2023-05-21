using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTtool.Models.Entity
{
    public class AccountModel
    {
        public short Id { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string AccountGroupId { get; set; }
        public string Name { get; set; }
    }
}