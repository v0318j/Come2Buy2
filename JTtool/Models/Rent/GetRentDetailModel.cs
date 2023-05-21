using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTtool.Models.Rent
{
    public class GetRentDetailModel
    {
        public short AId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class RentDetailModel
    {
        public int ExpenditureId { get; set; }
        public string Payer { get; set; }  
        public string Item { get; set; }
        public int Price { get; set; }
        public DateTime ExpenseDate { get; set; }
        public bool IsInstallment { get; set; }
        public short Periods { get; set; }
        public string Names { get; set; }
        public double PayAmount { get; set; }
        public bool IsAlways { get; set; }
        public short Creator { get; set; }
    }

    public class RentDetailVeiwModel
    {
        public int ExpenditureId {get; set; }
        public string Payer { get; set; }
        public string Item { get; set; }
        public string Price { get; set; }
        public string ExpenseDate { get; set; }
        public string IsInstallment { get; set; }
        public short Periods { get; set; }
        public string Names { get; set; }
        public string PayAmount { get; set; }
        public bool IsAlways { get; set; }
        public short Creator { get; set; }
    }
}