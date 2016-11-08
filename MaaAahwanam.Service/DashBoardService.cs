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
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<sp_AllOrders_Result> GetOrdersService(int id)
        {
            return dashBoardRepository.GetOrders(id);
        }

        public List<ServiceRequest> GetServicesService(int id)
        {
            return dashBoardRepository.GetServices(id);
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
        public List<serviceconfirmation_Result> GetParticularService(int id)
        {
            return dashBoardRepository.GetParticularService(id);
        }

        public List<sp_ServiceComments_Result> GetServiceComments(long id)
        {
            return dashBoardRepository.GetServiceComments(id);
        }

        public List<sp_QuotationComments_Result> GetQuotationComments(long id)
        {
            return dashBoardRepository.GetQuotationComments(id);
        }

        public long GetCommentId(string id)
        {
            return dashBoardRepository.GetCommentDetail(id);
        }

        public long GetQuotationCommentId(string id)
        {
            return dashBoardRepository.GetQuotationCommentDetail(id);
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

        public long GetCommentService(string id, string type)
        {
            return dashBoardRepository.GetComment(id, type);
        }

        public OrdersServiceRequest InsertNewOrderService(OrdersServiceRequest ordersServiceRequest)
        {
            return dashBoardRepository.InsertNewOrderService(ordersServiceRequest);
        }

        public Payments_Requests AddNewPaymentRequest(Payments_Requests payments_Requests)
        {
            return dashBoardRepository.AddNewPaymentRequest(payments_Requests);
        }

        public OrdersServiceRequest GetPaymentID(long id)
        {
            return dashBoardRepository.GetPaymentID(id);
        }

        public ServiceResponse GetService(long id)
        {
            return dashBoardRepository.GetQuotationList(id);
        }

        public OrdersServiceRequest UpdateOrdersServiceRequest(long id,OrdersServiceRequest ordersServiceRequest)
        {
            return dashBoardRepository.UpdateOrdersServiceRequest(id, ordersServiceRequest);
        }

        public ServiceRequest UpdateService(long id)
        {
            return dashBoardRepository.UpdateService(id);
        }

        public List<OrderDetail> GetPrice(long id)
        {
            return dashBoardRepository.GetPrice(id);
        }

        public List<ServiceRequest> GetParticularDate(long id)
        {
            return dashBoardRepository.GetParticularDate(id);
        }
    }
}
