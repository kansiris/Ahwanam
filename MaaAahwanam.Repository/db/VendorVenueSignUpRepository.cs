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
           var data= _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password && p.UserType == userLogin.UserType).FirstOrDefault();
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

        public VendorsCatering AddVendorCatering(VendorsCatering vendorsCatering)
        {
            _dbContext.VendorsCatering.Add(vendorsCatering);
            _dbContext.SaveChanges();
            return vendorsCatering;
        }

        public VendorVenue GetVendorVenue(long id)
        {
            return _dbContext.VendorVenue.Where(p => p.VendorMasterId == id).FirstOrDefault();
        }

        public VendorsCatering GetVendorCatering(long id)
        {
            return _dbContext.VendorsCatering.Where(p => p.VendorMasterId == id).FirstOrDefault();
        }

        public VendorsPhotography AddVendorPhotography(VendorsPhotography vendorsPhotography)
        {
            _dbContext.VendorsPhotography.Add(vendorsPhotography);
            _dbContext.SaveChanges();
            return vendorsPhotography;
        }

        public VendorsPhotography GetVendorPhotography(long id)
        {
            return _dbContext.VendorsPhotography.Where(p => p.VendorMasterId == id).FirstOrDefault();
        }

        public VendorsDecorator AddVendorDecorator(VendorsDecorator vendorsDecorator)
        {
            _dbContext.VendorsDecorator.Add(vendorsDecorator);
            _dbContext.SaveChanges();
            return vendorsDecorator;
        }

        public VendorsDecorator GetVendorDecorator(long id)
        {
            return _dbContext.VendorsDecorator.Where(p => p.VendorMasterId == id).FirstOrDefault();
        }

        public VendorsOther AddVendorOther(VendorsOther vendorsOther)
        {
            _dbContext.VendorsOther.Add(vendorsOther);
            _dbContext.SaveChanges();
            return vendorsOther;
        }

        public VendorsOther GetVendorOther(long id)
        {
            return _dbContext.VendorsOther.Where(p => p.VendorMasterId == id).FirstOrDefault();
        }


    }
}
