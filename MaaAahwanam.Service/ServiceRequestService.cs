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
    public class ServiceRequestService
    {
        ServiceRequestRepository serviceRequestRepository = new ServiceRequestRepository();
        public ServiceRequest SaveService(ServiceRequest serviceRequest)
        {
            serviceRequest = serviceRequestRepository.SaveQuotation(serviceRequest);
            return serviceRequest;
        }

        public List<ServiceRequest> GetServiceRequestList(ServiceRequest serviceRequest)
        {
            List<ServiceRequest> l1 = serviceRequestRepository.ServiceRequestList(serviceRequest);
            return l1;
        }
        public int GetServiceRequestListcount(string servicetype)
        {
            List<ServiceRequest> l1 = serviceRequestRepository.ServiceRequestListcount(servicetype);
            return l1.Count;
        }

        public List<ServiceRequest> GetServiceRequestRecord(ServiceRequest serviceRequest)
        {
            List<ServiceRequest> l1 = serviceRequestRepository.ServiceRequestRecord(serviceRequest);
            return l1;
        }

        public List<Vendormaster> getvendorslistRB(string stype)
        {
            List<Vendormaster> l1 = serviceRequestRepository.getvendorsluistRB(stype,"Venue");
            return l1;
        }
        public List<getservicetype_Result> getSubvendorslistRB(string stype)
        {
            List<getservicetype_Result> l2 = serviceRequestRepository.getSubvendorslistRB(stype);
            return l2;
        }

        public string getvendorname(long id)
        {
            return serviceRequestRepository.vendorname(id);
        }
        }
}
