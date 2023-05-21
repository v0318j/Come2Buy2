using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JTtool.Models.Rent
{
    public class AddExpenditureRequest
    {
        [Required(ErrorMessage = "請選擇付款者")]
        public short PayerId { get; set; }
        [Required(ErrorMessage = "請選擇分攤者")]
        public List<short> ShareIds { get; set; }
        [Required(ErrorMessage = "請輸入支出項目")]
        public string Item { get; set; }
        [Required(ErrorMessage = "請輸入價格")]
        public int Price { get; set; }
        [Required(ErrorMessage = "請輸入付款日期")]
        [JsonConverter(typeof(DateTime))]
        public DateTime ExpenseDate { get; set; }
        [Required(ErrorMessage = "請選擇是否分期")]
        public bool IsInstallment { get; set; }
        [Required(ErrorMessage = "請輸入分期期數")]
        public byte Periods { get; set; }
        [Required(ErrorMessage = "請選擇是否永遠付款")]
        public bool IsAlways { get; set; }
        public short AId { get; set; }
    }
}