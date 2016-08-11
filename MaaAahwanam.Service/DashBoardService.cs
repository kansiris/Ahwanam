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
    public class DashBoardService
    {
        DashBoardRepository dashBoardRepository = new DashBoardRepository();
        public List<sp_AllOrders_Result> GetOrdersService()
        {
            return dashBoardRepository.GetOrders();
        }

        public List<ServiceRequest> GetServicesService()
        {
            return dashBoardRepository.GetServices();
        }

        public List<sp_OrderDetails_Result> GetOrderDetailService(long id)
        {
            return dashBoardRepository.GetOrderDetail(id);
        }

        public List<sp_Servicedetails_Result> GetServiceDetailService(long id)
        {
            return dashBoardRepository.GetServiceDetail(id);
        }

        public List<sp_LeastBidRecord_Result> GetLeastBidService(long id)
        {
            return dashBoardRepository.GetLeastBid(id);
        }
    }
}
