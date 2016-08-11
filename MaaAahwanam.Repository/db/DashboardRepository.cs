using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using System.Data.SqlClient;

namespace MaaAahwanam.Repository.db
{
    public class DashBoardRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<sp_AllOrders_Result> GetOrders()
        {
            return maaAahwanamEntities.sp_AllOrders().ToList();
        }

        public List<ServiceRequest> GetServices()
        {
            return _dbContext.ServiceRequest.ToList();
        }

        public List<sp_OrderDetails_Result> GetOrderDetail(long id)
        {
            return maaAahwanamEntities.sp_OrderDetails(id).ToList();
        }

        public List<sp_Servicedetails_Result> GetServiceDetail(long id)
        {
            return maaAahwanamEntities.sp_Servicedetails(id).ToList();
        }

        public List<sp_LeastBidRecord_Result> GetLeastBid(long id)
        {
            return maaAahwanamEntities.sp_LeastBidRecord(id).ToList();
        }
    }
}
