using JTtool.Controllers.Filter;
using JTtool.Models;
using JTtool.Models.Entity;
using JTtool.Models.Home;
using JTtool.Services;
using JTtool.Services.Util;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace JTtool.Controllers
{
    [CheckLogin]
    public class AccountController : BaseController
    {
        AccountService AccountService = new AccountService();
        PasswordHasher PasswordHasher = new PasswordHasher();

        [HttpPost]
        public ActionResult AddAccount(AddAccountRequest request)
        {
            BaseResponse<object> response = new BaseResponse<object>();
            try
            {
                request.Password = PasswordHasher.HashPassword(request.Password);
                AccountService.AddAccount(request);
            }
            catch
            {
                response.Success = false;
                response.Message = "註冊失敗";
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordRequest request)
        {
            ChangePasswordResponse response = new ChangePasswordResponse();
            try
            {
                // 從 Session 中取得目前已登入的帳號
                AccountModel account = (AccountModel)Session[EnumType.Session.LoginAccount.ToString()];

                // 在允許密碼變更之前，先驗證目前的密碼
                PasswordHasher.VerifyPassword(request.CurrentPassword, account.Password);
                if (request.NewPassword != request.CheckPassword)
                {
                    response.Success = false;
                    response.Message = "新密碼需輸入一致";
                }
                if (PasswordHasher.VerifyPassword(request.CurrentPassword, account.Password))
                {
                    // 產生新的雜湊密碼
                    string newHashedPassword = PasswordHasher.HashPassword(request.NewPassword);

                    // 更新帳號的密碼
                    account.Password = newHashedPassword;

                    // 執行必要的帳號更新操作（例如，保存至資料庫）
                    AccountService.ChangePassword(LoggedInUserId.Value, newHashedPassword);
                }
            }
            catch
            {
                response.Success = false;
                response.Message = "密碼修改失敗";
            }

            return Json(response);
        }
    }
}