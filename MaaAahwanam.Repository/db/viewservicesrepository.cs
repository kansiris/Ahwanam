using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class viewservicesrepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public Vendormaster GetVendor(long id)
        {
            return _dbContext.Vendormaster.Where(m => m.Id == id).FirstOrDefault();
        }
        public List<VendorImage> GetVendorAllImages(long id)
        {
            return _dbContext.VendorImage.Where(m => m.VendorMasterId == id).ToList();
        }
        public List<VendorVenue> GetVendorVenue(long id)
        {
            return _dbContext.VendorVenue.Where(p => p.VendorMasterId == id).ToList();
        }
        public List<VendorsCatering> GetVendorCatering(long id)
        {
            return _dbContext.VendorsCatering.Where(p => p.VendorMasterId == id).ToList();
        }
        public List<VendorsDecorator> GetVendorDecorator(long id)
        {
            return _dbContext.VendorsDecorator.Where(p => p.VendorMasterId == id).ToList();
        }
        public List<VendorsPhotography> GetVendorPhotography(long id)
        {
            return _dbContext.VendorsPhotography.Where(p => p.VendorMasterId == id).ToList();
        }
        public List<VendorsOther> GetVendorOther(long id)
        {
            return _dbContext.VendorsOther.Where(p => p.VendorMasterId == id).ToList();
        }
        public List<SPGETNpkg_Result> getvendorpkgs(string id)
        {
            return maaAahwanamEntities.SPGETNpkg(Convert.ToInt64(id)).ToList();
        }
        public List<VendorDates> GetDates(long id, long subid)
        {
            return _dbContext.VendorDates.Where(m => m.VendorId == id && m.Vendorsubid == subid).ToList();
        }
        public List<VendorDates> GetCurrentMonthDates(long id)
        {
            var today = DateTime.UtcNow;
            var first = new DateTime(today.Year, today.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);
            return _dbContext.VendorDates.Where(m => m.VendorId == id && m.StartDate > first && m.EndDate <= last).ToList();
            //return _dbContext.Availabledates.Where(m => m.vendorId == id).ToList();
        }
        public string disabledate(long vid, long subid, string servicetype)
        {
            var today = DateTime.UtcNow;
            var first = new DateTime(today.Year, today.Month, 1);
            var orderid = _dbContext.OrderDetail.Where(m => m.VendorId == vid && m.subid == subid && m.ServiceType == servicetype && m.Status == "Active").Select(m => m.OrderId).ToList();
            var bookeddates = "";
            foreach (var item in orderid)
            {
                var dates = _dbContext.OrderDetail.FirstOrDefault(m => m.OrderId == item && m.BookedDate > first);
                if (dates != null)
                    bookeddates = bookeddates + "," + dates.BookedDate.Value.ToShortDateString();
                else
                    bookeddates = "";
            }
            bookeddates = String.Join(",", bookeddates.Split(',').Distinct());
            return bookeddates;
        }
        public List<OrderDetail> GetCount(long vid, long subid, string servicetype)
        {
            return _dbContext.OrderDetail.Where(m => m.VendorId == vid && m.subid == subid && m.ServiceType == servicetype).ToList();
        }
    }
}
