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
        NdealRepository NDealRep = new NdealRepository();

        public NDeals GetdealDetails(long dealId)
        {
            return NDealRep.GetdealDetails((dealId));
        }
    }
}
