using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
   public class VendorEventOrganiserRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> VendorsEventOrganiserList()
        {
            return _dbContext.VendorsEventOrganiser.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p = p, i = i }).ToList<dynamic>();

        }

        public VendorsEventOrganiser AddEventOrganiser(VendorsEventOrganiser vendorsEventOrganiser)
        {
            _dbContext.VendorsEventOrganiser.Add(vendorsEventOrganiser);
            _dbContext.SaveChanges();
            return vendorsEventOrganiser;
        }
        public VendorsEventOrganiser GetVendorEventOrganiser(long id, long vid)
        {
            return _dbContext.VendorsEventOrganiser.Where(m => m.VendorMasterId == id && m.Id == vid).FirstOrDefault();
        }

        public VendorsEventOrganiser UpdateEventOrganiser(VendorsEventOrganiser vendorsEventOrganiser, long id, long vid)
        {
            var GetVendor = _dbContext.VendorsEventOrganiser.SingleOrDefault(m => m.VendorMasterId == id && m.Id == vid);
            vendorsEventOrganiser.Id = GetVendor.Id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorsEventOrganiser);
            _dbContext.SaveChanges();
            return vendorsEventOrganiser;
        }

    }
}
