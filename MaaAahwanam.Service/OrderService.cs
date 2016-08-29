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
   public class OrderService
    {
        public List<sp_ordersdisplay_Result> OrderList()
        {
            OrderRepository orderRepository = new OrderRepository();
            return orderRepository.OrderList();
        }
        public List<MaaAahwanam_Orders_OrderDetails_Result> OrderDetailServivce(long id)
        {
            OrderRepository orderRepository = new OrderRepository();
            return orderRepository.GetOrderDetailsList(id);
        }
        public Order SaveOrder(Order order)
        {
            OrderRepository orderRepository = new OrderRepository();
            order = orderRepository.PostOrderDetails(order);
            return order;
        }
    }
}
