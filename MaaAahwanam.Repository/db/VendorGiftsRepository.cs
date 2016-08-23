using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
   public class VendorGiftsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> VendorsGiftsList()
        {
            return _dbContext.VendorsGift.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p = p, i = i }).ToList<dynamic>();

        }

        public VendorsGift AddGifts(VendorsGift vendorsGift)
        {
            _dbContext.VendorsGift.Add(vendorsGift);
            _dbContext.SaveChanges();
            return vendorsGift;
        }

        public VendorsGift GetVendorsGift(long id,long vid)
        {
            return _dbContext.VendorsGift.Where(m => m.VendorMasterId == id && m.Id == vid).FirstOrDefault();
        }

        public VendorsGift UpdatesGift(VendorsGift vendorsGift, long id,long vid)
        {
            var GetVendor = _dbContext.VendorsGift.SingleOrDefault(m => m.VendorMasterId == id && m.Id == vid);
            vendorsGift.Id = GetVendor.Id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorsGift);
            _dbContext.SaveChanges();
            return vendorsGift;
        }
    }
}
