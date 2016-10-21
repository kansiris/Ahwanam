using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class DealRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        
        public List<Deal> AllDeals(string dropstatus)
        {
            return _dbContext.Deal.Where(m=>m.VendorType == dropstatus).ToList();
        }

        public Deal GetDeal(int id)
        {
            return _dbContext.Deal.Where(m => m.DealID == id ).FirstOrDefault();
        }

        public Deal AddDeal(Deal deal)
        {
            _dbContext.Deal.Add(deal);
            _dbContext.SaveChanges();
            return deal;
        }

        public Deal UpdateDeal(Deal deal,int id)
        {
            var list = _dbContext.Deal.Where(m => m.DealID == id ).FirstOrDefault();
            deal.DealID = list.DealID;
            deal.VendorType = list.VendorType;
            deal.VendorId = list.VendorId;
            deal.VendorSubId = list.VendorSubId;
            deal.VendorCategory = list.VendorCategory;
            _dbContext.Entry(list).CurrentValues.SetValues(deal);
            _dbContext.SaveChanges();
            return deal;
        }
    }
}
