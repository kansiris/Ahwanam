using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class PaymentOrdersRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public Payment_Orders SavePayment_Orders(Payment_Orders payment_Orders)
        {
            _dbContext.Payment_Orders.Add(payment_Orders);
            _dbContext.SaveChanges();
            return payment_Orders;
        }

        public List<Payment_Orders> GetPaymentOrder(long id)
        {
           return _dbContext.Payment_Orders.Where(m => m.OrderID == id).ToList();
        }
    }
}
