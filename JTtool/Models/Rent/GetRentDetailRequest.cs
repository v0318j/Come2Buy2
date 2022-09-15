using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTtool.Models.Rent
{
    public class GetRentDetailRequest
    {
        public short AId { get; set; }
        public string YYMM { get; set; }
    }
}