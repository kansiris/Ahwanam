using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class ResultsPageService
    {
        ResultsPageRepository resultsPageRepository = new ResultsPageRepository();

        public List<GetVendors_Result> GetAllVendors(string type)
        {
            return resultsPageRepository.GetAllVendors(type);
        }
        public Vendormaster GetVendorByEmail(string emailid)
        {
            return resultsPageRepository.GetVendorByEmail(emailid);
        }
        public UserLogin GetUserLogdetails(UserLogin userLogin)
        {
            return resultsPageRepository.GetUserLogdetails(userLogin);
        }
        public UserLogin GetUserLogin(UserLogin userLogin)
        {
            return resultsPageRepository.GetUserLogin(userLogin);
        }
    }
}
