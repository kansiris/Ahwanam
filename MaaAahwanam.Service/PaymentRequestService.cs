using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class PaymentRequestService
    {
        PaymentRequestRepository paymentRequestRepository = new PaymentRequestRepository();
        public List<Payments_Requests> GetPaymentRequest(long id)
        {
            return paymentRequestRepository.GetPayments_Requests(id);
        }

        public List<ServiceResponse> GetServiceResponse(long id)
        {
            return paymentRequestRepository.GetServiceResponse(id);
        }
     }
}
