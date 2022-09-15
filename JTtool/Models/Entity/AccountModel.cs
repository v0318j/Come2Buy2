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
        public string Hash { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
    }
}