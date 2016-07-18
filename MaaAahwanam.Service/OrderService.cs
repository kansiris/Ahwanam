﻿using System;
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
        public List<Order> OrderList()
        {
            OrderRepository orderRepository = new OrderRepository();
            return orderRepository.OrderList();
        }
        public List<MaaAahwanam_Orders_OrderDetails_Result> OrderDetailServivce(long id)
        {
            OrderRepository orderRepository = new OrderRepository();
            return orderRepository.GetOrderDetailsList(id);
        }
    }
}
