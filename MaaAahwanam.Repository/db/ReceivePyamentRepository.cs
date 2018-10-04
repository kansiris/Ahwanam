using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class ReceivePyamentRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public Payment SavePayment(Payment payments)
        {
            _dbContext.Payment.Add(payments);
            _dbContext.SaveChanges();
            return payments;
        }
        public List<Payment> getPayments(string orderid)
        {
            
                var data = _dbContext.Payment.Where(p => p.OrderId == orderid).ToList(); // && p.UserType == userLogin.UserType
                return data;
            
        }
    }
}
