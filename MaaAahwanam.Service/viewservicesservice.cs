using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
   public class viewservicesservice
    {
        viewservicesrepository viewservicerepos = new viewservicesrepository();
        public Vendormaster GetVendor(long id)
        {
            return viewservicerepos.GetVendor(id);
        }
        public List<VendorImage> GetVendorAllImages(long id)
        {
            return viewservicerepos.GetVendorAllImages(id);
        }
        public List<VendorVenue> GetVendorVenue(long id)
        {
            return viewservicerepos.GetVendorVenue(id);
        }
        public VendorVenue GetParticularVendorVenue(long id, long vid)
        {
            return viewservicerepos.GetVendorVenue(id, vid);
        }
        public List<VendorsCatering> GetVendorCatering(long id)
        {
            return viewservicerepos.GetVendorCatering(id);
        }
        public List<VendorsDecorator> GetVendorDecorator(long id)
        {
            return viewservicerepos.GetVendorDecorator(id);
        }
        public List<VendorsPhotography> GetVendorPhotography(long id)
        {
            return viewservicerepos.GetVendorPhotography(id);
        }
        public List<VendorsOther> GetVendorOther(long id)
        {
            return viewservicerepos.GetVendorOther(id);
        }
        public List<SPGETNpkg_Result> getvendorpkgs(string id)
        {
            return viewservicerepos.getvendorpkgs(id);
        }
        public List<VendorDates> GetDates(long vendorid, long subid)
        {
            return viewservicerepos.GetDates(vendorid, subid);
        }
        public List<VendorDates> GetCurrentMonthDates(long id)
        {
            return viewservicerepos.GetCurrentMonthDates(id);
        }
        public string disabledate(long vid, long subid, string servicetype)
        {
            return viewservicerepos.disabledate(vid, subid, servicetype);
        }
        public List<OrderDetail> GetCount(long vid, long subid, string servicetype)
        {
            return viewservicerepos.GetCount(vid, subid, servicetype);
        }
        public GetProductsInfo_Result getProductsInfo_Result(int vid, string servicetype, int Subvid)
        {
            return viewservicerepos.getProductsInfo(vid, servicetype, Subvid);
        }
     
    }
}
