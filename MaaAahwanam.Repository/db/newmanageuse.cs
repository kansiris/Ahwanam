using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MaaAahwanam.Repository.db
{
  public  class newmanageuse
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public Vendormaster GetVendorByEmail(string emailid)
        {
            return _dbContext.Vendormaster.Where(m => m.EmailId == emailid).FirstOrDefault();
        }
        public string username(long UserId)
        {
            return _dbContext.UserLogin.Where(p => p.UserLoginId == UserId).Select(u => u.UserName).FirstOrDefault();
        }
        public UserDetail GetLoginDetailsByUsername(int userId)
        {
            UserDetail list = new UserDetail();
            if (userId != 0)
                list = _dbContext.UserDetail.SingleOrDefault(p => p.UserLoginId == userId);
            return list;
        }
        public Vendormaster getvendor(int vendorid)
        {
            Vendormaster list = new Vendormaster();
            if (vendorid != 0)
                list = _dbContext.Vendormaster.SingleOrDefault(p => p.Id == vendorid);
            return list;
        }
        public List<SPGETpartpkg_Result> getpartpkgs(long id)
        {
            return maaAahwanamEntities.SPGETpartpkg(id).ToList();
        }

        public Order PostOrderDetails(Order order)
        {
            _dbContext.Order.Add(order);
            _dbContext.SaveChanges();
            return order;
        }
        public OrderDetail SaveOrderDetail(OrderDetail orderDetail)
        {
            _dbContext.OrderDetail.Add(orderDetail);
            _dbContext.SaveChanges();
            return orderDetail;
        }
        public StaffAccess Savestaff(StaffAccess Staffsccess)
        {
            _dbContext.StaffAccess.Add(Staffsccess);
            _dbContext.SaveChanges();
            return Staffsccess;
        }
        public UserLogin addloginstaff(UserLogin userLogin)
        {
            _dbContext.UserLogin.Add(userLogin);
            _dbContext.SaveChanges();
            return userLogin;
        }

        public List<sp_userorddisplay_Result> userOrderList()
        {
            return maaAahwanamEntities.sp_userorddisplay().ToList();
        }
        public List<sp_vendoruserorddisplay_Result> userOrderList1()
        {
            return maaAahwanamEntities.sp_vendoruserorddisplay().ToList();
        }
        public Order updateOrderstatus(Order order, OrderDetail orderdetail, long orderid)
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
        public StaffAccess updatestaff(StaffAccess Staffsccess, long id)
        {

            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.StaffAccess
                where ord.ID == id
                select ord;
            // Query the database for the row to be updated.

            // Execute the query, and change the column values
            // you want to change.
            foreach (StaffAccess ord in query)
            {
                ord.Status = Staffsccess.Status;
                // Insert any additional changes to column values.
            }
            // Query the database for the row to be updated.
         
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return Staffsccess;
        }
    }
}
