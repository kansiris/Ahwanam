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
           var data= _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password).FirstOrDefault(); // && p.UserType == userLogin.UserType
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

        public List<VendorVenue> GetVendorVenue(long id)
        {
            return _dbContext.VendorVenue.Where(p => p.VendorMasterId == id).ToList();
        }

        public List<VendorsCatering> GetVendorCatering(long id)
        {
            return _dbContext.VendorsCatering.Where(p => p.VendorMasterId == id).ToList();
        }

        public VendorsPhotography AddVendorPhotography(VendorsPhotography vendorsPhotography)
        {
            _dbContext.VendorsPhotography.Add(vendorsPhotography);
            _dbContext.SaveChanges();
            return vendorsPhotography;
        }

        public List<VendorsPhotography> GetVendorPhotography(long id)
        {
            return _dbContext.VendorsPhotography.Where(p => p.VendorMasterId == id).ToList();
        }

        public VendorsDecorator AddVendorDecorator(VendorsDecorator vendorsDecorator)
        {
            _dbContext.VendorsDecorator.Add(vendorsDecorator);
            _dbContext.SaveChanges();
            return vendorsDecorator;
        }

        public List<VendorsDecorator> GetVendorDecorator(long id)
        {
            return _dbContext.VendorsDecorator.Where(p => p.VendorMasterId == id).ToList();
        }

        public VendorsOther AddVendorOther(VendorsOther vendorsOther)
        {
            _dbContext.VendorsOther.Add(vendorsOther);
            _dbContext.SaveChanges();
            return vendorsOther;
        }

        public List<VendorsOther> GetVendorOther(long id)
        {
            return _dbContext.VendorsOther.Where(p => p.VendorMasterId == id).ToList();
        }


    }
}
