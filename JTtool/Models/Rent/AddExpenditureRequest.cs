using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JTtool.Models.Rent
{
    public class AddExpenditureRequest
    {
        public short PayerId { get; set; }
        public string Item { get; set; }
        public int Price { get; set; }
        [JsonConverter(typeof(DateTime))]
        public DateTime ExpenseDate { get; set; }
        public bool IsInstallment { get; set; }
        public byte Periods { get; set; }
        public bool IsAlways { get; set; }
        public List<short> ShareIds { get; set; }
        public short AId { get; set; }
    }
}