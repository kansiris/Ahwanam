using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class PaymentRequestRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public Payments_Requests Payments_Requests(Payments_Requests payments_Requests)
        {
            _dbContext.Payments_Requests.Add(payments_Requests);
            _dbContext.SaveChanges();
            return payments_Requests;
        }

        public List<Payments_Requests> GetPayments_Requests(long id)
        {
            return _dbContext.Payments_Requests.Where(m => m.RequestID == id).ToList();
        }
    }
}
