using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTtool.Models.Rent
{
    public class GetRentDetailResponse : BaseResponse
    {
        public short Payable { get; set; }
        public short Paid { get; set; }
        public List<RentDetailVeiwModel> RentDetail { get; set; }
        public double Rent { get; set; }
    }
}