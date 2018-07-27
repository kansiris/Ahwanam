using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Repository.db
{
    public class OrderdetailsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public OrderDetail SaveOrderDetail(OrderDetail orderDetail)
        {
            _dbContext.OrderDetail.Add(orderDetail);
            _dbContext.SaveChanges();
            return orderDetail;
        }

        public List<OrderDetail> GetCount(long vid,long subid,string servicetype)
        {
            return _dbContext.OrderDetail.Where(m => m.VendorId == vid && m.subid == subid && m.ServiceType == servicetype).ToList();
        }

        public long? ratingscount(long vid, long subid, string servicetype)
        {
            return maaAahwanamEntities.ratingscount(vid,subid,servicetype).FirstOrDefault();
        }

        public List<VendorsDatesbooked_Result> DatesBooked(int id)
        {
            var list = _dbContext.UserLogin.Where(m => m.UserLoginId == id).FirstOrDefault();
            var list1 = _dbContext.Vendormaster.Where(m => m.EmailId == list.UserName).FirstOrDefault();
            return maaAahwanamEntities.VendorsDatesbooked((int)list1.Id).ToList();
        }

        public List<SP_Amenities_Result> GetAmenities(long subid, string type)
        {
            return maaAahwanamEntities.SP_Amenities(subid, type).ToList();
        }
        public int OrdersCount(long id)
        {
            long orderid = _dbContext.OrderDetail.Where(m => m.OrderDetailId == id).Select(m=>m.OrderId).FirstOrDefault();
            return _dbContext.OrderDetail.Where(m => m.OrderId == orderid).Count();
        }
        public string disabledate(long vid, long subid, string servicetype)
        {
            var today = DateTime.UtcNow;
            var first = new DateTime(today.Year, today.Month, 1);
            var orderid = _dbContext.OrderDetail.Where(m => m.VendorId == vid && m.subid == subid && m.ServiceType == servicetype).Select(m=>m.OrderId).ToList();
            var bookeddates = "";
            foreach (var item in orderid)
            {
                var dates = _dbContext.OrderDetail.FirstOrDefault(m => m.OrderId == item && m.BookedDate > first);
                if (dates != null)
                    bookeddates = bookeddates + "," + dates.BookedDate.Value.ToShortDateString();
                else
                    bookeddates = "";
            }
            bookeddates = String.Join(",", bookeddates.Split(',').Distinct());
            return bookeddates;
        }
    }
}
