using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class ResultsPageRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();

        public List<GetVendors_Result> GetAllVendors(string type)
        {
            return maaAahwanamEntities.GetVendors(type).ToList();
        }
        public List<GetFilteredVendors_Result> GetVendorsByName(string type, string name)
        {
            return maaAahwanamEntities.GetFilteredVendors(type,name).ToList();
        }
        public Vendormaster GetVendorByEmail(string emailid)
        {
            return _dbContext.Vendormaster.Where(m => m.EmailId == emailid).FirstOrDefault();
        }
        public UserLogin GetUserLogdetails(UserLogin userLogin)
        {
            var data = _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName).FirstOrDefault(); // && p.UserType == userLogin.UserType
            return data;
        }
        public UserLogin GetUserLogin(UserLogin userLogin)
        {
            var data = _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password).FirstOrDefault(); // && p.UserType == userLogin.UserType
            return data;
        }

        public List<GetPhotographers_Result> GetAllPhotographers()
        {
            return maaAahwanamEntities.GetPhotographers().ToList();
        }

        public List<GetDecorators_Result> GetAllDecorators()
        {
            return maaAahwanamEntities.GetDecorators().ToList();
        }
    }
}
