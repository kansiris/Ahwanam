using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendorDatesRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public VendorDates SaveVendorDates(VendorDates vendorDates)
        {
            vendorDates = _dbContext.VendorDates.Add(vendorDates);
            _dbContext.SaveChanges();
            return vendorDates;
        }

        public List<VendorDates> GetDates(long id, long subid)
        {
            return _dbContext.VendorDates.Where(m => m.VendorId == id && m.Vendorsubid == subid ).ToList();
        }

        public string removedates(long id)
        {
            try
            {
                var list = _dbContext.VendorDates.FirstOrDefault(m => m.Id == id);
                _dbContext.VendorDates.Remove(list);
                _dbContext.SaveChanges();
                return "Removed";
            }
            catch
            {
                return "Failed!!!";
            }
        }

        public VendorDates UpdatesVendorDates(VendorDates vendorDates, long id)
        {
            var GetVendor = _dbContext.VendorDates.SingleOrDefault(m => m.Id == id);
            vendorDates.Id = GetVendor.Id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorDates);
            _dbContext.SaveChanges();
            return vendorDates;
        }

        public VendorDates GetParticularDate(long id)
        {
            return _dbContext.VendorDates.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
