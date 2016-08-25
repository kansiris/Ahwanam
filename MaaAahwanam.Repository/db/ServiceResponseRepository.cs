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
   public class ServiceResponseRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public long ServiceResponseCount(ServiceResponse serviceResponse)
        {
            return _dbContext.ServiceResponse.Where(m => m.RequestId == serviceResponse.RequestId).Count();
        }

        public List<MaaAahwanam_Services_Bidding_Result> ServiceResponseList(long id)
        {
            return maaAahwanamEntities.MaaAahwanam_Services_Bidding(id).ToList();
        }

        public List<ServiceResponse> GetQuotationList(ServiceResponse serviceResponse)
        {
            return _dbContext.ServiceResponse.Where(m => m.RequestId == serviceResponse.RequestId).ToList();
        }

        public ServiceResponse SaveServiceResponse(ServiceResponse serviceResponse)
        {
            _dbContext.ServiceResponse.Add(serviceResponse);
            _dbContext.SaveChanges();
            return serviceResponse;
        }
        public List<SP_vendordatesbooked_Result> GetVendordatesbooked(int VID)
        {
            return maaAahwanamEntities.SP_vendordatesbooked(VID).ToList();
        }
        public ServiceResponse UpdateServiceResponse(ServiceResponse serviceResponse)
        {
            var GetRecord = _dbContext.ServiceResponse.Where(m => m.ResponseId == serviceResponse.ResponseId).FirstOrDefault();
            _dbContext.Entry(GetRecord).CurrentValues.SetValues(serviceResponse);
            _dbContext.SaveChanges();
            return serviceResponse;
        }

    }
}
