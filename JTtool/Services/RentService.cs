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
    public class RentService : BaseService
    {
        public List<RentDetailModel> GetRentDetail(GetRentDetailModel model)
        {
            int lastyear = 0, lastmonth = 0;
            if (model.Month == 1)
            {
                lastyear = model.Year - 1;
                lastmonth = 12;
            }
            else
            {
                lastyear = model.Year;
                lastmonth = model.Month - 1;
            }

            DateTime lastMonth = new DateTime(lastyear, lastmonth, 20);
            DateTime thisMonth = new DateTime(model.Year, model.Month, 19);

            List<RentDetailModel> details = new List<RentDetailModel>();

            var expenditure = from e in db.Expenditure
                              where (e.IsInstallment
                                    ? SqlFunctions.DateAdd("month", e.Periods - 1, e.ExpenseDate) >= lastMonth && e.ExpenseDate <= thisMonth
                                    : e.ExpenseDate >= lastMonth && e.ExpenseDate <= thisMonth)
                                    || (e.IsAlways ? thisMonth >= e.ExpenseDate : e.IsAlways)
                              select e;

            details.AddRange((from e in (from e in expenditure
                                         join payer in db.Account on e.Payer equals payer.Id
                                         orderby e.ExpenseDate descending
                                         select new
                                         {
                                             e.Id,
                                             PayerId = e.Payer,
                                             Payer = payer.Name,
                                             e.Item,
                                             e.Price,
                                             e.ExpenseDate,
                                             e.IsInstallment,
                                             e.Periods,
                                             e.IsAlways,
                                             e.Creator
                                         }).ToList()
                              join s in (from e in expenditure
                                         join es in db.ExpenditureShare on e.Id equals es.ExpenditureId
                                         join a in db.Account on es.AccountId equals a.Id
                                         select new
                                         {
                                             e.Id,
                                             payerId = e.Payer,
                                             shareId = es.AccountId,
                                             name = a.Name
                                         })
                                         .ToList()
                                         .GroupBy(i => i.Id)
                                         .Where(i => i.Where(j => j.payerId == model.AId).Count() != 0 || i.Where(j => j.shareId == model.AId).Count() != 0)
                                         .Select(i => new
                                         {
                                             Id = i.Key,
                                             Names = string.Join(",", i.Select(j => j.name)),
                                             Count = i.Count()
                                         }).ToList() on e.Id equals s.Id
                              orderby e.ExpenseDate descending
                              select new RentDetailModel
                              {
                                  ExpenditureId = e.Id,
                                  Payer = e.Payer,
                                  Item = e.Item,
                                  Price = e.Price,
                                  ExpenseDate = e.ExpenseDate,
                                  IsInstallment = e.IsInstallment,
                                  Periods = e.Periods,
                                  Names = s.Names,
                                  PayAmount = (double)e.Price / (e.Periods == 0 ? 1 : e.Periods) / (s.Count + 1) * (e.PayerId == model.AId ? -1 : 1),
                                  IsAlways = e.IsAlways,
                                  Creator= e.Creator
                              }).ToList());

            return details;
        }

        public GetExpenditureResponseData GetExpenditure(int id)
        {
            return db.Expenditure.Where(i => i.Id == id).Select(i => new GetExpenditureResponseData
            {
                Id = i.Id,
                ExpenseDate = i.ExpenseDate,
                IsAlways = i.IsAlways,
                IsInstallment = i.IsInstallment,
                Item = i.Item,
                Price = i.Price,
                PayerId = i.Payer,
                Periods = i.Periods,
                ShareIds = db.ExpenditureShare.Where(j => j.ExpenditureId == id).Select(j => j.AccountId).ToList(),
            }).Single();
        }

        public void AddExpenditure(AddExpenditureRequest request)
        {
            Expenditure expenditure = new Expenditure
            {
                Payer = request.PayerId,
                ExpenseDate = request.ExpenseDate,
                Item = request.Item,
                Price = request.Price,
                IsInstallment = request.IsInstallment,
                Periods = request.Periods,
                IsAlways = request.IsAlways,
                Creator = request.AId,
                CreateDatetime = DateTime.Now
            };
            db.Expenditure.Add(expenditure);
            db.SaveChanges();

            List<ExpenditureShare> expenditureShare = new List<ExpenditureShare>();
            foreach (short shareId in request.ShareIds)
            {
                expenditureShare.Add(new ExpenditureShare
                {
                    ExpenditureId = expenditure.Id,
                    AccountId = shareId
                });
            }
            db.ExpenditureShare.AddRange(expenditureShare);
            db.SaveChanges();
        }
        public void UpdateExpenditure(UpdateExpenditureRequest request, short loggedInUserId)
        {
            // 取得要修改的支出項目
            Expenditure expenditure = db.Expenditure.FirstOrDefault(e => e.Id == request.ExpenditureId);
            if (expenditure == null)
            {
                // 支出項目不存在，可能需要採取相應的錯誤處理措施
                return;
            }
            if (expenditure.Creator != loggedInUserId)
            {
                throw new Exception("不可刪除他人建立之資料");
            }

            // 更新支出項目的屬性
            expenditure.Payer = request.PayerId;
            expenditure.ExpenseDate = request.ExpenseDate;
            expenditure.Item = request.Item;
            expenditure.Price = request.Price;
            expenditure.IsInstallment = request.IsInstallment;
            expenditure.Periods = request.Periods;
            expenditure.IsAlways = request.IsAlways;

            // 刪除原有的支出項目分享關係
            List<ExpenditureShare> existingShares = db.ExpenditureShare.Where(es => es.ExpenditureId == request.ExpenditureId).ToList();
            db.ExpenditureShare.RemoveRange(existingShares);

            // 建立新的支出項目分享關係
            List<ExpenditureShare> expenditureShares = new List<ExpenditureShare>();
            foreach (short shareId in request.ShareIds)
            {
                expenditureShares.Add(new ExpenditureShare
                {
                    ExpenditureId = request.ExpenditureId,
                    AccountId = shareId
                });
            }
            db.ExpenditureShare.AddRange(expenditureShares);

            // 儲存變更
            db.SaveChanges();
        }
        public void DeleteExpenditure(DeleteExpenditureRequest request, short loggedInUserId)
        {
            // 取得要刪除的支出項目
            Expenditure expenditure = db.Expenditure.FirstOrDefault(e => e.Id == request.ExpenditureId);
            if (expenditure == null)
            {
                // 支出項目不存在，可能需要採取相應的錯誤處理措施
                return;
            }
            if (expenditure.Creator != loggedInUserId)
            {
                throw new Exception("不可刪除他人建立之資料");
            }

            // 刪除支出項目分享關係s
            List<ExpenditureShare> expenditureShares = db.ExpenditureShare.Where(es => es.ExpenditureId == request.ExpenditureId).ToList();
            db.ExpenditureShare.RemoveRange(expenditureShares);

            // 刪除支出項目
            db.Expenditure.Remove(expenditure);

            // 儲存變更
            db.SaveChanges();
        }
    }
}