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
        public List<ServiceRequest> GetParticularService(long id)
        {
            return dashBoardRepository.GetParticularService(id);
        }

        public List<sp_ServiceComments_Result> GetServiceComments(long id)
        {
            return dashBoardRepository.GetServiceComments(id);
        }

        public long GetCommentId(string id)
        {
            return dashBoardRepository.GetCommentDetail(id);
        }

        public string GetServiceType(long id)
        {
            return dashBoardRepository.GetServiceType(id);
        }

        public Comment InsertCommentService(Comment comment)
        {
            comment.UpdatedDate = DateTime.Now;
            comment.Status = "Active";
            return dashBoardRepository.InsertComment(comment);
        }

        public CommentDetail InsertCommentDetailService(CommentDetail commentDetail)
        {
            commentDetail.CommentDate = DateTime.Now;
            commentDetail.UpdatedDate = DateTime.Now;
            commentDetail.Status = "Active";
            return dashBoardRepository.InsertCommentDetail(commentDetail);
        }
    }
}
