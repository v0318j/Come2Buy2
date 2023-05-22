using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Antlr.Runtime.Tree;
using JTtool.Models;
using JTtool.Models.Entity;
using JTtool.Models.Home;
using JTtool.Models.Rent;
using JTtool.Services.Util;

namespace JTtool.Services
{
    public class AccountService : BaseService
    {
        public AccountModel Login(LoginRequest request) =>
            db.Account
            .Where(i => i.AccountGroupId == EnumType.AccountGroup.Rent.ToString() && i.LoginId == request.LoginId)
            .Select(i => new AccountModel
            {
                Id = i.Id,
                LoginId = i.LoginId,
                Password= i.Password,
                Name = i.Name
            })
            .Single();

        public IEnumerable<AccountModel> GetRentUsers()
        {
            IEnumerable<Account> rentUsers = db.Account
                .Where(i => i.AccountGroupId == EnumType.AccountGroup.Rent.ToString())
                .ToList();

            return rentUsers.Select(i => new AccountModel
            {
                Id = i.Id,
                LoginId = i.LoginId,
                Name = i.Name
            });
        }

        public void ChangePassword(short accountId, string newPassword)
        {
            Account account = db.Account.Single(i => i.Id == accountId);

            account.Password = newPassword;

            db.SaveChanges();
        }

        public void AddAccount(AddAccountRequest request)
        {
            if(db.Account.Any(i => i.LoginId == request.LoginId))
            {
                throw new CustomException("帳號已存在");
            }
            Account account = new Account
            {
                LoginId = request.LoginId,
                AccountGroupId = EnumType.AccountGroup.Rent.ToString(),
                Name = request.Name,
                Password = request.Password,
                Creator = db.Account.Single(i => i.LoginId == "System").Id,
                CreateDatetime= DateTime.UtcNow
            };
            db.Account.Add(account);
            db.SaveChanges();
        }
    }
}