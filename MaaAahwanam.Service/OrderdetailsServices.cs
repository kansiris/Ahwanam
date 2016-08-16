using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;

namespace MaaAahwanam.Service
{
    public class OrderdetailsServices
    {
        public OrderDetail SaveOrderDetail(OrderDetail orderDetail)
        {
            OrderdetailsRepository orderdetailsRepository = new OrderdetailsRepository();
            orderDetail = orderdetailsRepository.SaveOrderDetail(orderDetail);
            return orderDetail;
        }
    }
}
