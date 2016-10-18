using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Repository.db
{
    public class ServiceRequestRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public ServiceRequest SaveQuotation(ServiceRequest serviceRequest)
        {
            _dbContext.ServiceRequest.Add(serviceRequest);
            _dbContext.SaveChanges();
            return serviceRequest;
        }

        public List<ServiceRequest> ServiceRequestList(ServiceRequest serviceRequest)
        {
            return _dbContext.ServiceRequest.Where(m => m.Type == serviceRequest.Type).ToList();
        }
        public List<ServiceRequest> ServiceRequestListcount(string Servicecount)
        {
            return _dbContext.ServiceRequest.Where(m => m.Type == Servicecount).ToList();
        }

        public List<ServiceRequest> ServiceRequestRecord(ServiceRequest serviceRequest)
        {
            return _dbContext.ServiceRequest.Where(m => m.RequestId == serviceRequest.RequestId).ToList();
        } 

        public ServiceRequest UpdateBidStatus(ServiceRequest serviceRequest)
        {
            
            return serviceRequest;
        }
        public List<Vendormaster> getvendorsluistRB(String stype,string type)
        {
            var l1 = maaAahwanamEntities.getservicetype(type).Where(m=>m.vendortype==stype).Select(d=>d.temp).ToList();
            List<Vendormaster> ss=_dbContext.Vendormaster.Where(m=>l1.Contains(m.Id)).ToList();
            return ss;
        }
        public List<getservicetype_Result> getSubvendorslistRB(String stype)
        {
            return maaAahwanamEntities.getservicetype(stype).ToList();
        }

        public string vendorname(long id)
        {
            return _dbContext.Vendormaster.Where(m => m.Id == id).Select(m => m.BusinessName).FirstOrDefault() ;
        }
    }
}
