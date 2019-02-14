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
        public List<GetFilteredVendors_Result> GetVendorsByName(string type, string name)
        {
            return resultsPageRepository.GetVendorsByName(type, name);
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

        public List<GetPhotographers_Result> GetAllPhotographers()
        {
            return resultsPageRepository.GetAllPhotographers();
        }

        public List<GetDecorators_Result> GetAllDecorators()
        {
            return resultsPageRepository.GetAllDecorators();
        }

        public List<GetCaterers_Result> GetAllCaterers()
        {
            return resultsPageRepository.GetAllCaterers();
        }

        public List<GetOthers_Result> GetAllOthers(string type)
        {
            return resultsPageRepository.GetAllOthers(type);
        }
    }
}
