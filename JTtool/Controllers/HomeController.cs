using JTtool.Models.Entity;
using JTtool.Models.Home;
using JTtool.Services;
using System.Web;
using System.Web.Mvc;

namespace JTtool.Controllers
{
    public class HomeController : Controller
    {
        HomeService HomeService = new HomeService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            string errMsg = CheckLogin(request);
            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                return Json(new LoginResponse { Success = false, Message = errMsg });
            }
            else
            {
                try
                {
                    Session.Add(EnumType.Session.LoginAccount.ToString(), HomeService.Login(request));
                    return Json(new LoginResponse { Success = true, Message = ((AccountModel)Session[EnumType.Session.LoginAccount.ToString()]).Id.ToString(), Redirect = "Rent" });
                }
                catch
                {
                    return Json(new LoginResponse { Success = false, Message = "帳號密碼有誤！" });
                }
            }
        }

        private string CheckLogin(LoginRequest request)
        {
            //先不用密碼
            if (string.IsNullOrWhiteSpace(request.LoginId))
            {
                return "請輸入帳號密碼！";
            }
            return "";
        }
    }
}