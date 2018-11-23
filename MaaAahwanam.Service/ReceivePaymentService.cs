using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Service
{
   public class ReceivePaymentService
    {
        ReceivePyamentRepository rcvpmntrepo = new ReceivePyamentRepository();
        public Payment SavePayments(Payment payments)
        {
            payments = rcvpmntrepo.SavePayment(payments);
            return payments;
        }
        public List<Payment> getPayments(string orderid)
        {
            var payments = rcvpmntrepo.GetPaydetails(orderid);
            return payments;
        }
        public List<Payment> getPaymentsbyodid(string orderdetailid)
        {
            var paymentsbyodid = rcvpmntrepo.getPaymentsbyodid(orderdetailid);
            return paymentsbyodid;
        }
        public List<Payment> Getpaymentby(string oid, string paymentby)
        {
            var paymentbycustomer = rcvpmntrepo.Getpaymentby(oid, paymentby);
            return paymentbycustomer;
        }
    }
}
