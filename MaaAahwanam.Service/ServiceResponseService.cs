using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using System.Data.SqlClient;

namespace MaaAahwanam.Service
{
    public class ServiceResponseService
    {
        ServiceResponseRepository serviceResponseRepository = new ServiceResponseRepository();
        public long ServiceResponseCount(ServiceResponse serviceResponse)
        {
            return serviceResponseRepository.ServiceResponseCount(serviceResponse);
        }

        //public List<dynamic> GetServiceResponseList(ServiceResponse serviceResponse)
        //{
        //    ServiceResponseRepository serviceResponseRepository = new ServiceResponseRepository();
        //    return serviceResponseRepository.ServiceResponseList(serviceResponse);
        //}
        public List<MaaAahwanam_Services_Bidding_Result> GetServiceResponseList(long id)
        {
            return serviceResponseRepository.ServiceResponseList(id);
        }

        public List<ServiceResponse> GetQuotationList(ServiceResponse serviceResponse)
        {
            return serviceResponseRepository.GetQuotationList(serviceResponse);
        }

        public string SaveServiceResponse(ServiceResponse serviceResponse)
        {
            var a = serviceResponseRepository.SaveServiceResponse(serviceResponse);
            string message = "";
            if (a.ResponseId != 0)
            {
                message = "Success";
            }
            else
            {
                message = "Failed to save";
            }
            return message;
        }
        public List<SP_vendordatesbooked_Result> GetVendordatesbooked(int VID)
        {
            return serviceResponseRepository.GetVendordatesbooked(VID).ToList();
        }
        public string UpdateServiceResponse(ServiceResponse serviceResponse)
        {
            var a = serviceResponseRepository.UpdateServiceResponse(serviceResponse);
            string message = "";
            if (a.ResponseId != 0)
            {
                message = "Success";
            }
            else
            {
                message = "Failed to save";
            }
            return message;
        }
        public List<ServiceResponse> BidHistory(long ResponseID)
        {
            return serviceResponseRepository.BidHistory(ResponseID);
        }
    }
}
