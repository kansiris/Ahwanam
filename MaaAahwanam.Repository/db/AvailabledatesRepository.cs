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
        public Availabledates saveavailabledates(Availabledates availabledates)
        {
            availabledates = _dbContext.Availabledates.Add(availabledates);
            _dbContext.SaveChanges();
            return availabledates;
        }

        public List<Availabledates> GetDates(long id)
        {
            return _dbContext.Availabledates.Where(m => m.vendorId == id).ToList();
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
    }
}
