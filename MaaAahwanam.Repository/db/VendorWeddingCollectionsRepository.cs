using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
   public class VendorWeddingCollectionsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> VendorsWeddingCollectionsList()
        {
            return _dbContext.VendorsWeddingCollection.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p = p, i = i }).ToList<dynamic>();

        }

        public VendorsWeddingCollection AddWeddingCollections(VendorsWeddingCollection vendorsWeddingCollections)
        {
            _dbContext.VendorsWeddingCollection.Add(vendorsWeddingCollections);
            _dbContext.SaveChanges();
            return vendorsWeddingCollections;
        }
        public VendorsWeddingCollection GetVendorWeddingCollection(long id, long vid)
        {
            return _dbContext.VendorsWeddingCollection.Where(m => m.VendorMasterId == id && m.Id == vid).FirstOrDefault();
        }

        public VendorsWeddingCollection UpdateWeddingCollection(VendorsWeddingCollection vendorsWeddingCollection, long id, long vid)
        {
            var GetVendor = _dbContext.VendorsWeddingCollection.SingleOrDefault(m => m.VendorMasterId == id && m.Id == vid);
            vendorsWeddingCollection.Id = GetVendor.Id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorsWeddingCollection);
            _dbContext.SaveChanges();
            return vendorsWeddingCollection;
        }
    }
}
