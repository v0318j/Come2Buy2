using JTtool.Controllers.Filter;
using JTtool.Models;
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
    public class RentController : BaseController
    {
        AccountService AccountService = new AccountService();
        RentService RentService = new RentService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRentDetail(GetRentDetailRequest request)
        {
            GetRentDetailResponse response = new GetRentDetailResponse
            {
                Data = new GetRentDetailResponseData()
            };
            if (DateTime.TryParse(request.YYMM, out DateTime yymm))
            {
                List<RentDetailModel> detail = RentService.GetRentDetail(new GetRentDetailModel
                {
                    Year = yymm.Year,
                    Month = yymm.Month
                }, LoggedInUserId);

                response.Data.Rent = detail.Count > 1 ? Math.Round(detail.Select(i => i.PayAmount).Aggregate((i, j) => i + j)) :
                    detail.Count == 1 ? Math.Round(detail[0].PayAmount) : 0;
                response.Data.RentDetail = detail.Select(i => new RentDetailVeiwModel
                {
                    ExpenditureId = i.ExpenditureId,
                    Payer = i.Payer,
                    Item = i.Item,
                    Price = i.Price.ToString("$#,##0"),
                    ExpenseDate = i.ExpenseDate.ToString("yyyy-MM-dd"),
                    IsInstallment = i.IsInstallment ? "是" : "否",
                    Periods = i.Periods,
                    Names = i.Names,
                    PayAmount = i.PayAmount.ToString("$#,##0.00"),
                    IsAlways = i.IsAlways,
                    Creator = i.Creator
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

        [HttpGet]
        public JsonResult GetExpenditure(int id)
        {
            GetExpenditureResponse response = new GetExpenditureResponse();
            try
            {
                response.Data = RentService.GetExpenditure(id);
            }
            catch
            {
                response.Success = false;
                response.Message = "查詢失敗";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddExpenditure(AddExpenditureRequest request)
        {
            BaseResponse<object> response = new BaseResponse<object>();
            try
            {
                CheckExpenditure(request.PayerId, request.ShareIds);
                RentService.AddExpenditure(request, LoggedInUserId);
            }
            catch (CustomException e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            catch
            {
                response.Success = false;
                response.Message = "新增失敗";
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult UpdateExpenditure(UpdateExpenditureRequest request)
        {
            BaseResponse<object> response = new BaseResponse<object>();
            try
            {
                CheckExpenditure(request.PayerId, request.ShareIds);
                RentService.UpdateExpenditure(request, LoggedInUserId);
            }
            catch (CustomException e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            catch
            {
                response.Success = false;
                response.Message = "修改失敗";
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult DeleteExpenditure(DeleteExpenditureRequest request)
        {
            BaseResponse<object> response = new BaseResponse<object>();
            try
            {
                RentService.DeleteExpenditure(request, LoggedInUserId);
            }
            catch (CustomException e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            catch
            {
                response.Success = false;
                response.Message = "刪除失敗";
            }
            return Json(response);
        }

        [HttpGet]
        public ActionResult GetRentUsers()
        {
            AccountResponse response = new AccountResponse();
            try
            {
                response.Data = AccountService.GetRentUsers();
            }
            catch
            {
                response.Success = false;
                response.Message = "查詢失敗";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private void CheckExpenditure(short PayerId, List<short> ShareIds)
        {
            if (ShareIds.Any(i => i == PayerId))
            {
                throw new CustomException("分攤者不可包含付款者");
            }
            if (!ShareIds.Any(i => i == LoggedInUserId) && PayerId != LoggedInUserId)
            {
                throw new CustomException("不可新增與自己無關的資料");
            }
        }
    }
}