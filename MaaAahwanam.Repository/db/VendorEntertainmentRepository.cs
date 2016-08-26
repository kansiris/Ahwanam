using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
   public class VendorEntertainmentRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> VendorsEntertainmentList()
        {
            return _dbContext.VendorsEntertainment.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p = p, i = i }).ToList<dynamic>();

        }

        public VendorsEntertainment AddEntertainment(VendorsEntertainment vendorsEntertainment)
        {
            _dbContext.VendorsEntertainment.Add(vendorsEntertainment);
            _dbContext.SaveChanges();
            return vendorsEntertainment;
        }
        public VendorsEntertainment GetVendorEntertainment(long id, long vid)
        {
            return _dbContext.VendorsEntertainment.Where(m => m.VendorMasterId == id && m.Id == vid).FirstOrDefault();
        }

        public VendorsEntertainment UpdateEntertainment(VendorsEntertainment vendorsEntertainment, long id, long vid)
        {
            var GetVendor = _dbContext.VendorsEntertainment.SingleOrDefault(m => m.VendorMasterId == id && m.Id == vid);
            vendorsEntertainment.Id = GetVendor.Id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorsEntertainment);
            _dbContext.SaveChanges();
            return vendorsEntertainment;
        }
    }
}
