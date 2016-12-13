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
    public class OrderdetailsServices
    {
        OrderdetailsRepository orderdetailsRepository = new OrderdetailsRepository();
        public OrderDetail SaveOrderDetail(OrderDetail orderDetail)
        {
            orderDetail = orderdetailsRepository.SaveOrderDetail(orderDetail);
            return orderDetail;
        }
        public List<VendorsDatesbooked_Result> DatesBooked(int id)
        {
            return orderdetailsRepository.DatesBooked(id);
        }
    }
}
