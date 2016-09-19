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
        public List<sp_AllOrders_Result> GetOrders(int id)
        {
            return maaAahwanamEntities.sp_AllOrders(id).ToList();
        }

        public List<ServiceRequest> GetServices(int id)
        {
            return _dbContext.ServiceRequest.Where(m=>m.UpdatedBy == id).ToList();
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

        public List<sp_QuotationComments_Result> GetQuotationComments(long id)
        {
            return maaAahwanamEntities.sp_QuotationComments(id).ToList();
        }

        public long GetCommentDetail(string id)
        {
            
            return _dbContext.Comment.Where(m => m.ServiceId == id).Select(r => r.CommentId).FirstOrDefault();
           
        }

        public long GetQuotationCommentDetail(string id)
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

        public long GetComment(string id, string type)
        {
            return _dbContext.Comment.Where(m => m.ServiceId == id && m.ServiceType == type).Count();
        }

        public OrdersServiceRequest InsertNewOrderService(OrdersServiceRequest ordersServiceRequest)
        {
            _dbContext.OrdersServiceRequest.Add(ordersServiceRequest);
            _dbContext.SaveChanges();
            return ordersServiceRequest;
        }

        public Payments_Requests AddNewPaymentRequest(Payments_Requests payments_Requests)
        {
            _dbContext.Payments_Requests.Add(payments_Requests);
            _dbContext.SaveChanges();
            return payments_Requests;
        }

        public OrdersServiceRequest GetPaymentID(long id)
        {
            return _dbContext.OrdersServiceRequest.Where(m => m.ResponseId == id).FirstOrDefault();
        }

        public ServiceResponse GetQuotationList(long id)
        {
            return _dbContext.ServiceResponse.Where(m => m.ResponseId == id).FirstOrDefault();
        }

        public OrdersServiceRequest UpdateOrdersServiceRequest(long id,OrdersServiceRequest ordersServiceRequest)
        {
            var GetRecord = _dbContext.Payments_Requests.Where(m => m.RequestID == id).FirstOrDefault();
            //ordersServiceRequest
            ordersServiceRequest.PaymentId = GetRecord.PaymentID;
            
            _dbContext.Entry(GetRecord).CurrentValues.SetValues(ordersServiceRequest);
            _dbContext.SaveChanges();
            return ordersServiceRequest;
        }

        public ServiceRequest UpdateService(long id)
        {
            ServiceRequest serviceRequest;
            var GetRecord=_dbContext.ServiceRequest.Where(m => m.RequestId == id).FirstOrDefault();
            serviceRequest = _dbContext.ServiceRequest.Where(m => m.RequestId == id).FirstOrDefault();
            serviceRequest.Status = "Paid";
            _dbContext.Entry(GetRecord).CurrentValues.SetValues(serviceRequest);
            _dbContext.SaveChanges();
            return serviceRequest;
        }
    }
}
