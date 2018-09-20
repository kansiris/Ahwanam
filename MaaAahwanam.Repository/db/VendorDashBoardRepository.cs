using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
  public class VendorDashBoardRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public ManageVendor AddVendor(ManageVendor mngvendor)
        {
            _dbContext.ManageVendor.Add(mngvendor);
            _dbContext.SaveChanges();
            return mngvendor;
        }
        public List<ManageVendor> GetVendorList(string Vid)
        {
            return _dbContext.ManageVendor.Where(v => v.vendorId == Vid).ToList();
        }
        //public int GetSubVendorId(string Vid)
        //{
        //    var query = from Vendorsubid in _dbContext.ManageVendor where ManageVendor.vendorId == Vid select ManageVendor.id;
            
        //}
        public ManageVendor UpdateVendor(ManageVendor vendor,int id)
        {
            ManageVendor mngvendor = _dbContext.ManageVendor.Where(v => v.id == id).SingleOrDefault();
            mngvendor.firstname = vendor.firstname;
            mngvendor.lastname = vendor.lastname;
            mngvendor.phoneno = vendor.phoneno;
            mngvendor.services = vendor.services;
            mngvendor.state = vendor.state;
            mngvendor.Status = vendor.Status;
            mngvendor.city = vendor.city;
            mngvendor.country = vendor.country;
            mngvendor.Businessname = vendor.Businessname;
            mngvendor.adress1 = vendor.adress1;
            mngvendor.adress2 = vendor.adress2;
            mngvendor.email = vendor.email;
            mngvendor.registereddate = vendor.registereddate;
            mngvendor.updatedby = vendor.vendorId;
            _dbContext.SaveChanges();
            return mngvendor;
        }
        public ManageVendor GetVendordetails(int id)
        {
            //var query = from vendor in _dbContext.ManageVendor where vendor.id == id select vendor;
            return _dbContext.ManageVendor.Where(v => v.id == id).FirstOrDefault();

        }
        public int checkvendoremail(string email,int id)
        {
            int c = _dbContext.ManageVendor.Where(e => e.email == email && e.id == id).Count();
            return c;
        }
        public ManageUser AddUser(ManageUser mnguser)
        {
            _dbContext.ManageUser.Add(mnguser);
            _dbContext.SaveChanges();
            return mnguser;

        }
        public List<ManageUser> GetUserList(string Vid)
        {
            return _dbContext.ManageUser.Where(v => v.vendorId == Vid).ToList();
        }

    }
}
