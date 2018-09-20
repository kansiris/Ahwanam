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
