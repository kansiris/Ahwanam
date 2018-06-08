using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
   public class VendorImageService
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorImageRepository vendorImageRepository = new VendorImageRepository();
        public VendorImage AddVendorImage(VendorImage vendorImage, Vendormaster vendorMaster)
       {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            vendorImage.Status = "Active";
           vendorImage.UpdatedDate = updateddate;
           vendorImage.VendorMasterId = vendorMaster.Id;
           vendorImage = vendorImageRepository.AddVendorImage(vendorImage);
           return vendorImage;
       }

        public List<string> GetVendorImagesService(long id,long vid)
        {
            return vendorImageRepository.GetVendorImages(id,vid);
        }
        public string DeleteImage(VendorImage vendorImage)
        {
            return vendorImageRepository.DeleteImage(vendorImage);
        }
        public VendorImage GetImageId(string name,long vid)
        {
            return vendorImageRepository.GetImageId(name,vid);
        }
        public string UpdateVendorVenue(VendorImage vendorImage)
        {
            return vendorImageRepository.UpdateVendorVenue(vendorImage);
        }
        public List<VendorImage> GetVendorAllImages(long id)
        {
            return vendorImageRepository.GetVendorAllImages(id);
        }
        public List<VendorImage> GetImages(long id, long vid)
        {
            return vendorImageRepository.GetImages(id,vid);
        }
        public string UpdateVendorImage(VendorImage vendorImage, long id, long vid)
        {
            return vendorImageRepository.UpdateVendorImage(vendorImage, id, vid);
        }
        public string DeleteAllImages(long id, long vid)
        {
            return vendorImageRepository.DeleteAllImages(id,vid);
        }
    }
}

