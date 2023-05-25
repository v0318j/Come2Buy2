using System.Web;
using System.Web.Mvc;
using JTtool.Services;

namespace JTtool.Controllers.Filter
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if(session[EnumType.Session.LoginAccount.ToString()] == null)
            {
                filterContext.Result = new RedirectResult("/Home");
                return;
            }
        }
    }
}