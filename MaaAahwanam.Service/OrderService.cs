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
        OrderRepository orderRepository = new OrderRepository();

        public List<sp_ordersdisplay_Result> OrderList()
        {
            return orderRepository.OrderList();
        }
        public List<sp_userorddisplay_Result> userOrderList()
        {
            return orderRepository.userOrderList();
        }
        public List<MaaAahwanam_Orders_OrderDetails_Result> OrderDetailServivce(long id)
        {
            return orderRepository.GetOrderDetailsList(id);
        }
        public Order SaveOrder(Order order)
        {
            order = orderRepository.PostOrderDetails(order);
            return order;
        }

        public Order updateOrderstatus(Order order,OrderDetail orderdetail, long orderid)
        {
            var changes = orderRepository.updateOrderstatus(order, orderdetail,orderid);
            return changes;
        }

        public Order GetParticularOrder(long id)
        {
            return orderRepository.GetParticularOrder(id);
        }
    }
}
