using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class OrderConfirmationRepository
    {
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<orderconfirmation_Result> GetOrderConfirmation(int OID)
        {
            var list= maaAahwanamEntities.orderconfirmation(OID).ToList();
            return list;
        }
    }
}
