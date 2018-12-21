using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Service
{

    public class Ndealservice
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        NdealRepository NDealRep = new NdealRepository();

        public NDeals GetdealDetails(long dealId)
        {
            return NDealRep.GetdealDetails((dealId));
        }

        public NDeals AddDeal(NDeals deal)
        {
            deal.UpdatedDate = updateddate;
            deal.Status = "Active";
            return NDealRep.AddDeal(deal);
        }

        public List<NDeals> GetAllDeals()
        {
            return NDealRep.GetAllDeals();
        }
    }
}
