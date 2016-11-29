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
    }
}
