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
    }
}
