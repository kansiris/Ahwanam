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

        public List<Payment> Getpmntdetails(string oid)
        {
            List<Payment> payments = new List<Payment>();
           payments= rcvpmntrepo.GetPaydetails(oid);
            return payments;
        }
    }
}
