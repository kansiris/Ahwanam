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

        public List<ServiceRequest> GetParticularService(long id)
        {
            return _dbContext.ServiceRequest.Where(m => m.RequestId == id).ToList();
        }

        public List<sp_ServiceComments_Result> GetServiceComments(long id)
        {
            return maaAahwanamEntities.sp_ServiceComments(id).ToList();
        }

        public long GetCommentDetail(string id)
        {
            return _dbContext.Comment.Where(m => m.ServiceId == id).Select(r => r.CommentId).FirstOrDefault();
        }

        public Comment InsertComment(Comment comment)
        {
            _dbContext.Comment.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public CommentDetail InsertCommentDetail(CommentDetail commentDetail)
        {
            _dbContext.CommentDetail.Add(commentDetail);
            _dbContext.SaveChanges();
            return commentDetail;
        }

        public string GetServiceType(long id)
        {
            return _dbContext.ServiceRequest.Where(m => m.RequestId == id).Select(r => r.Type).FirstOrDefault();
        }
    }
}
