using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Repository.db
{
   public class OrderRepository
    {
       readonly ApiContext _dbContext = new ApiContext();
       MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
       public List<Order> OrderList()
       {
           return _dbContext.Order.ToList();
       }
       
       public List<MaaAahwanam_Orders_OrderDetails_Result> GetOrderDetailsList(long id)
       {
           return maaAahwanamEntities.MaaAahwanam_Orders_OrderDetails(id).ToList();
       }

        public Order PostOrderDetails(Order order)
        {
            _dbContext.Order.Add(order);
            _dbContext.SaveChanges();
            return order;
        }
    }
}
