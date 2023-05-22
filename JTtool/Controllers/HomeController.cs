using JTtool.Models.Entity;
using JTtool.Models.Home;
using JTtool.Services;
using JTtool.Services.Util;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace JTtool.Controllers
{
    public class HomeController : BaseController
    {
        AccountService AccountService = new AccountService();
        PasswordHasher PasswordHasher = new PasswordHasher();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            try
            {
                AccountModel account = AccountService.Login(request);

                PasswordHasher.VerifyPassword(request.Password, account.Password);
                Session.Add(EnumType.Session.LoginAccount.ToString(), account);
                return Json(new LoginResponse
                {
                    Data = new LoginResponseData
                    {
                        Redirect = "/Rent/Index"
                    }
                });
            }
            catch
            {
                return Json(new LoginResponse { Success = false, Message = "帳號密碼有誤！" });
            }
        }
    }
}