using System.Linq;
using System.Security.Policy;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using JTtool.Models.Entity;
using JTtool.Models.Home;

namespace JTtool.Services
{
    public class HomeService : BaseService
    {
        public AccountModel Login(LoginRequest request) =>
            db.Account.Where(i => i.AccountGroupId == EnumType.AccountGroup.Rent.ToString() && i.LoginId == request.LoginId)
                .Select(i => new AccountModel
                {
                    Id = i.Id,
                    LoginId = i.LoginId,
                    Hash = i.Hash,
                    Salt = i.Salt,
                    Name = i.Name
                }).Single();
    }
}