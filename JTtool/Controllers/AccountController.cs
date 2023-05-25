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
                if (request.Password != request.ConfirmPassword)
                {
                    throw new CustomException("密碼需輸入一致");
                }
                request.Password = PasswordHasher.HashPassword(request.Password);
                AccountService.AddAccount(request);
            }
            catch (CustomException e)
            {
                response.Success = false;
                response.Message = e.Message;
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
                AccountModel account = (AccountModel)Session[EnumType.Session.LoginAccount.ToString()];

                if (request.NewPassword != request.CheckPassword)
                {
                    throw new CustomException("密碼需輸入一致");
                }
                if (PasswordHasher.VerifyPassword(request.CurrentPassword, account.Password))
                {
                    string newHashedPassword = PasswordHasher.HashPassword(request.NewPassword);

                    account.Password = newHashedPassword;

                    AccountService.ChangePassword(LoggedInUserId.Value, newHashedPassword);
                }
            }
            catch (CustomException e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            catch
            {
                response.Success = false;
                response.Message = "密碼修改失敗";
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            return new RedirectResult("/Home");
        }
    }
}