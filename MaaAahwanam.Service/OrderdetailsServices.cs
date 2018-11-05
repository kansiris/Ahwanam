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
        public int OrdersCount(long id)
        {
            return orderdetailsRepository.OrdersCount(id);
        }

        public List<OrderDetail> GetOrderDetails(string oid)
        {
            var orderdetails = orderdetailsRepository.GetOrderDetails(Convert.ToInt64(oid));
            return orderdetails;
        }

        public OrderDetail GetOrderDetailsByOrderdetailid(long odid)
        {
            return orderdetailsRepository.GetOrderDetailsbyOrderDetailId(odid);
        }
    }
}
