using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendorImageRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<VendorImage> VendorImageList(VendorImage vendorImage)
        {
            return _dbContext.VendorImage.ToList();
        }

        public VendorImage AddVendorImage(VendorImage vendorImage)
        {
            _dbContext.VendorImage.Add(vendorImage);
            _dbContext.SaveChanges();
            return vendorImage;
        }

        public List<string> GetVendorImages(long id, long vid)
        {
            return _dbContext.VendorImage.Where(m =>  m.VendorMasterId == id && m.VendorId == vid).Select(p => p.ImageName).ToList();
        }
        public string DeleteImage(VendorImage vendorImage)
        {
            _dbContext.VendorImage.Remove(vendorImage);
            _dbContext.SaveChanges();
            return "success";
        }

        public VendorImage GetImageId(string name)
        {
            return _dbContext.VendorImage.Where(m => m.ImageName == name).SingleOrDefault();
        }

        public string UpdateVendorVenue(VendorImage vendorImage)
        {
            _dbContext.Entry(vendorImage).CurrentValues.SetValues(vendorImage);
            _dbContext.SaveChanges();
            return "updated";
        }
    }
}
