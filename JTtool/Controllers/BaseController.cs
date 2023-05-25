using JTtool.Models;
using JTtool.Models.Entity;
using JTtool.Models.Home;
using JTtool.Services;
using JTtool.Services.Util;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JTtool.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        protected short? LoggedInUserId { get
            {
                return ((AccountModel)Session[EnumType.Session.LoginAccount.ToString()])?.Id;
            }
        } 

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new BaseResponse<object>
                    {
                        Success = false,
                        Message = GetErrorMessageFromModelState(filterContext.Controller.ViewData.ModelState)
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        private string GetErrorMessageFromModelState(ModelStateDictionary modelState)
        {
            var errorMessage = string.Join("<br>", modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            return errorMessage;
        }
    }
}