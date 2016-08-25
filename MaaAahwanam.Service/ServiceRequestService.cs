using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class ServiceRequestService
    {
        public ServiceRequest SaveService(ServiceRequest serviceRequest)
        {
            ServiceRequestRepository serviceRequestRepository = new ServiceRequestRepository();
            serviceRequest = serviceRequestRepository.SaveQuotation(serviceRequest);
            return serviceRequest;
        }

        public List<ServiceRequest> GetServiceRequestList(ServiceRequest serviceRequest)
        {
            ServiceRequestRepository serviceRequestRepository = new ServiceRequestRepository();
            List<ServiceRequest> l1 = serviceRequestRepository.ServiceRequestList(serviceRequest);
            return l1;
        }

        public List<ServiceRequest> GetServiceRequestRecord(ServiceRequest serviceRequest)
        {
            ServiceRequestRepository serviceRequestRepository = new ServiceRequestRepository();
            List<ServiceRequest> l1 = serviceRequestRepository.ServiceRequestRecord(serviceRequest);
            return l1;
        }

        public List<Vendormaster> getvendorslistRB(string stype)
        {
            ServiceRequestRepository serviceRequestRepository = new ServiceRequestRepository();
            List<Vendormaster> l1 = serviceRequestRepository.getvendorsluistRB(stype);
            return l1;
        }
    }
}
