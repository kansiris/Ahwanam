using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendorsTravelandAccomodationRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> VendorsTravelandAccomodationList()
        {
            return _dbContext.VendorsTravelandAccomodation.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p = p, i = i }).ToList<dynamic>();

        }

        public VendorsTravelandAccomodation AddTravelandAccomodation(VendorsTravelandAccomodation vendorsTravelandAccomodation)
        {
            _dbContext.VendorsTravelandAccomodation.Add(vendorsTravelandAccomodation);
            _dbContext.SaveChanges();
            return vendorsTravelandAccomodation;
        }
        public VendorsTravelandAccomodation GetVendorTravelandAccomodation(long id, long vid)
        {
            return _dbContext.VendorsTravelandAccomodation.Where(m => m.VendorMasterId == id && m.Id == vid).FirstOrDefault();
        }

        public VendorsTravelandAccomodation UpdateTravelandAccomodation(VendorsTravelandAccomodation vendorsTravelandAccomodation, long id, long vid)
        {
            var GetVendor = _dbContext.VendorsTravelandAccomodation.SingleOrDefault(m => m.VendorMasterId == id && m.Id == vid);
            vendorsTravelandAccomodation.Id = GetVendor.Id;
            vendorsTravelandAccomodation.VendorMasterId = id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorsTravelandAccomodation);
            _dbContext.SaveChanges();
            return vendorsTravelandAccomodation;
        }
    }
}
