using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendorVenueSignUpRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public UserLogin AddUserLogin(UserLogin userLogin)
        {
            _dbContext.UserLogin.Add(userLogin);
            _dbContext.SaveChanges();
            return userLogin;
        }

        public UserLogin GetUserLogin(UserLogin userLogin)
        {
           var data= _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password).FirstOrDefault();
            return data;
        }

        public Vendormaster AddVendormaster(Vendormaster vendormaster)
        {
            _dbContext.Vendormaster.Add(vendormaster);
            _dbContext.SaveChanges();
            return vendormaster;
        }

        public UserDetail AddUserDetail(UserDetail userDetail)
        {
            _dbContext.UserDetail.Add(userDetail);
            _dbContext.SaveChanges();
            return userDetail;
        }

        public VendorVenue AddVendorVenue(VendorVenue vendorVenue)
        {
            _dbContext.VendorVenue.Add(vendorVenue);
            _dbContext.SaveChanges();
            return vendorVenue;
        }
    }
}
