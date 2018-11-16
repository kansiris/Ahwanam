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
       public List<sp_ordersdisplay_Result> OrderList()
       {
            return maaAahwanamEntities.sp_ordersdisplay().ToList();
       }
        public List<Order> getorder()
        {
            //Order list = new Order();

            return _dbContext.Order.ToList();
           
        }

        public List<sp_userorddisplay_Result> userOrderList()
        {
            return maaAahwanamEntities.sp_userorddisplay().ToList();
        }
        public List<sp_allorddetail_Result> alluserOrderList(string type)
        {
            return maaAahwanamEntities.sp_allorddetail(type).ToList();
        }
        public List<sp_userordlist_Result> allorderslist()
        {
            return maaAahwanamEntities.sp_userordlist().ToList();
        }
        public List<sp_vendoruserorddisplay_Result> userOrderList1()
        {
            return maaAahwanamEntities.sp_vendoruserorddisplay().ToList();
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
        public Order updateOrderstatus(Order order,OrderDetail orderdetail, long orderid)
        {
            
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.Order
                where ord.OrderId == orderid
                select ord;
            // Query the database for the row to be updated.
           
            // Execute the query, and change the column values
            // you want to change.
            foreach (Order ord in query)
            {
                ord.Status = order.Status;
                // Insert any additional changes to column values.
            }
            // Query the database for the row to be updated.
            var query1 =
                from ord1 in _dbContext.OrderDetail
                where ord1.OrderId == orderid
                select ord1;
            // Query the database for the row to be updated.

            // Execute the query, and change the column values
            // you want to change.
            foreach (OrderDetail ord1 in query1)
            {
                ord1.Status = order.Status;
                // Insert any additional changes to column values.
            }
            // Submit the changes to the database.
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return order;
        }

        public Order GetParticularOrder(long id)
        {
            return _dbContext.Order.Where(m => m.OrderId == id).FirstOrDefault();
        }
    }
}

