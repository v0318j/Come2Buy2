using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Linq;
using Antlr.Runtime.Tree;
using JTtool.Models.Entity;
using JTtool.Models.Home;
using JTtool.Models.Rent;

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
                Hash = i.Hash,
                Salt = i.Salt,
                Name = i.Name
            })
            .Single();

        public IEnumerable<AccountResponse> GetRentUsersExceptLoggedIn(short loggedInUserId)
        {
            IEnumerable<Account> rentUsers = db.Account
                .Where(i => i.AccountGroupId == EnumType.AccountGroup.Rent.ToString() && i.Id != loggedInUserId)
                .ToList();

            return rentUsers.Select(i => new AccountResponse
            {
                Id = i.Id,
                LoginId = i.LoginId,
                Name = i.Name
            });
        }
    }
}