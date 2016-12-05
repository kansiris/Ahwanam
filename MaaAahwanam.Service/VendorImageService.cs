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
        VendorImageRepository vendorImageRepository = new VendorImageRepository();
        public VendorImage AddVendorImage(VendorImage vendorImage, Vendormaster vendorMaster)
       {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorImage.Status = "Active";
           vendorImage.UpdatedDate = Convert.ToDateTime(updateddate);
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
    }
}
