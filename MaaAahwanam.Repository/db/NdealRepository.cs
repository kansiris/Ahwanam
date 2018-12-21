using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{

    public class NdealRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public NDeals GetdealDetails(long dealId)
        {
            NDeals list1 = new NDeals();
            if (dealId != 0)
                list1 = _dbContext.NDeal.SingleOrDefault(p => p.DealID == dealId);
            return list1;
        }

        public NDeals AddDeal(NDeals deal)
        {
            _dbContext.NDeal.Add(deal);
            _dbContext.SaveChanges();
            return deal;
        }

        public List<NDeals> GetAllDeals()
        {
            return _dbContext.NDeal.ToList();
        }
    }
}


