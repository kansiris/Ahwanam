using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
   public class VendorOthersRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> VendorsOthersList()
        {
            return _dbContext.VendorsOther.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p = p, i = i }).ToList<dynamic>();

        }

        public VendorsOther AddOthers(VendorsOther vendorsOthers)
        {
            _dbContext.VendorsOther.Add(vendorsOthers);
            _dbContext.SaveChanges();
            return vendorsOthers;
        }
        public VendorsOther GetVendorOthers(long id, long vid)
        {
            return _dbContext.VendorsOther.Where(m => m.VendorMasterId == id && m.Id == vid).FirstOrDefault();
        }

        public VendorsOther UpdateOthers(VendorsOther vendorsOther, long id, long vid)
        {
            var GetVendor = _dbContext.VendorsOther.SingleOrDefault(m => m.VendorMasterId == id && m.Id == vid);
            vendorsOther.Id = GetVendor.Id;
            vendorsOther.VendorMasterId = id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorsOther);
            _dbContext.SaveChanges();
            return vendorsOther;
        }
    }
}
