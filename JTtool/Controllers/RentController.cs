using JTtool.Controllers.Filter;
using JTtool.Models.Entity;
using JTtool.Models.Home;
using JTtool.Models.Rent;
using JTtool.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace JTtool.Controllers
{
    [CheckLogin]
    public class RentController : Controller
    {
        AccountService AccountService = new AccountService();
        RentService RentService = new RentService();
        public ActionResult Index(short AId)
        {
            ViewBag.AId = AId;
            return View();
        }

        [HttpPost]
        public JsonResult GetRentDetail(GetRentDetailRequest request)
        {
            GetRentDetailResponse response = new GetRentDetailResponse();
            if (DateTime.TryParse(request.YYMM, out DateTime yymm))
            {
                List<RentDetailModel> detail = RentService.GetRentDetail(new GetRentDetailModel
                {
                    AId = request.AId,
                    Year = yymm.Year,
                    Month = yymm.Month
                });

                response.Rent = detail.Count > 1 ? Math.Round(detail.Select(i => i.PayAmount).Aggregate((i, j) => i + j)) :
                    detail.Count == 1 ? Math.Round(detail[0].PayAmount) : 0;
                response.RentDetail = detail.Select(i => new RentDetailVeiwModel
                {
                    Payer = i.Payer,
                    Item = i.Item,
                    Price = i.Price.ToString("$#,##0"),
                    ExpenseDate = i.ExpenseDate.ToShortDateString(),
                    IsInstallment = i.IsInstallment ? "是" : "否",
                    Periods = i.Periods,
                    Names = i.Names,
                    PayAmount = i.PayAmount.ToString("$#,##0.00")
                }).ToList();

                foreach (RentDetailModel d in detail)
                {
                    d.PayAmount = Math.Round(d.PayAmount, 2);
                }
            }
            else
            {
                response.Success = false;
                response.Message = "輸入日期有誤";
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult AddExpenditure(AddExpenditureRequest request)
        {
            RentService.AddExpenditure(request);
            return Json("OK");
        }

        [HttpPost]
        public JsonResult UpdateExpenditure(UpdateExpenditureRequest request)
        {
            RentService.UpdateExpenditure(request);
            return Json("OK");
        }

        [HttpPost]
        public JsonResult DeleteExpenditure(DeleteExpenditureRequest request)
        {
            RentService.DeleteExpenditure(request);
            return Json("OK");
        }

        [HttpGet]
        public ActionResult GetRentUsersExceptLoggedIn()
        {
            short loggedInUserId = ((AccountModel)Session[EnumType.Session.LoginAccount.ToString()]).Id;

            IEnumerable<AccountResponse> rentUsers = AccountService.GetRentUsersExceptLoggedIn(loggedInUserId);

            return Json(rentUsers, JsonRequestBehavior.AllowGet);
        }
    }
}