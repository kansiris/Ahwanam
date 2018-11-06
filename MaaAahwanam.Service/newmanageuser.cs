using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Repository.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Service
{
   public class newmanageuser
    {
        newmanageuse newusermanager = new newmanageuse();
        public Vendormaster GetVendorByEmail(string emailid)
        {
            return newusermanager.GetVendorByEmail(emailid);
        }

        public string Getusername(long UserId)
        {
            return newusermanager.username(UserId);
        }
        public UserDetail GetUser(int userid)
        {
            string response = string.Empty;
            UserDetail list = newusermanager.GetLoginDetailsByUsername(userid);
            return list;
        }
        public Vendormaster getvendor(int vendorid)
        {
            string response = string.Empty;
            Vendormaster list = newusermanager.getvendor(vendorid);
            return list;

        }
        public List<SPGETpartpkg_Result> getpartpkgs(string id)
        {
            return newusermanager.getpartpkgs(Convert.ToInt64(id));
        }
        public Order SaveOrder(Order order)
        {
            order = newusermanager.PostOrderDetails(order);
            return order;
        }
        public OrderDetail SaveOrderDetail(OrderDetail orderDetail)
        {
            orderDetail = newusermanager.SaveOrderDetail(orderDetail);
            return orderDetail;
        }
        public StaffAccess Savestaff(StaffAccess Staffsccess)
        {
            Staffsccess = newusermanager.Savestaff(Staffsccess);
            return Staffsccess;
        }
        public UserLogin addloginstaff(UserLogin userLogin)
        {
            userLogin = newusermanager.addloginstaff(userLogin);
            return userLogin;
        }
        public List<sp_userorddisplay_Result> userOrderList()
        {
            return newusermanager.userOrderList();
        }
        public List<sp_vendoruserorddisplay_Result> userOrderList1()
        {
            return newusermanager.userOrderList1();
        }
        public Order updateOrderstatus(Order order, OrderDetail orderdetail, long orderid)
        {
            var changes = newusermanager.updateOrderstatus(order, orderdetail, orderid);
            return changes;
        }
        public StaffAccess updatestaff(StaffAccess Staffsccess, long id)
        {
            var changes = newusermanager.updatestaff(Staffsccess, id);
            return changes;
        }
    }
}
