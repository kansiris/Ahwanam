using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class AvailabledatesRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public Availabledates saveavailabledates(Availabledates availabledates)
        {
            availabledates = _dbContext.Availabledates.Add(availabledates);
            _dbContext.SaveChanges();
            return availabledates;
        }

        public List<Availabledates> GetDates(long id, long subid)
        {
            var today = DateTime.UtcNow;
            var first = new DateTime(today.Year, today.Month , 1);
            var last = today.AddMonths(2);
            var lastday = last.AddDays(-(last.Day));
            return _dbContext.Availabledates.Where(m => m.vendorId == id && m.vendorsubid == subid && m.availabledate > first && m.availabledate < lastday).ToList();
            //return _dbContext.Availabledates.Where(m => m.vendorId == id).ToList();
        }

        public string removedates(Availabledates availabledates, long id)
        {
            try
            {
                var list = _dbContext.Availabledates.FirstOrDefault(m => m.vendorId == id && m.availabledate == availabledates.availabledate);
                _dbContext.Availabledates.Remove(list);
                _dbContext.SaveChanges();
                return "Removed";
            }
            catch
            {
                return "Failed!!!";
            }
        }

        public List<Availabledates> GetCurrentMonthDates(long id)
        {
            var today = DateTime.UtcNow;
            var first = new DateTime(today.Year, today.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);
            return _dbContext.Availabledates.Where(m => m.vendorId == id && m.availabledate > first && m.availabledate <= last).ToList();
            //return _dbContext.Availabledates.Where(m => m.vendorId == id).ToList();
        }

        public List<vendorallservices_Result> VendorAllServices(string type, long id)
        {
            return maaAahwanamEntities.vendorallservices(type, id).ToList();
        }
    }
}
