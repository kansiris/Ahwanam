using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;

namespace MaaAahwanam.Service
{
    public class Payment_orderServices
    {
        PaymentOrdersRepository paymentOrdersRepository = new PaymentOrdersRepository();
        public Payment_Orders SavePayment_Orders(Payment_Orders payment_Orders)
        {
            payment_Orders = paymentOrdersRepository.SavePayment_Orders(payment_Orders);
            return payment_Orders;
        }

        public List<Payment_Orders> GetPaymentOrderService(long id)
        {
            return paymentOrdersRepository.GetPaymentOrder(id);
        }
    }
}
