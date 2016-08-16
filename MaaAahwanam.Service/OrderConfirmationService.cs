using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class OrderConfirmationService
    {
        public List<orderconfirmation_Result> GetOrderConfirmation()
        {
            OrderConfirmationRepository orderConfirmationRepository = new OrderConfirmationRepository();
            var list = orderConfirmationRepository.GetOrderConfirmation().ToList<orderconfirmation_Result>();
            return list;
        }
    }
}
