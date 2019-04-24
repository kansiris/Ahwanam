using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class FiltersRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<Category> AllCategories()
        {
            return _dbContext.Category.ToList();
        }

        public Category category(int categoryid)
        {
            return _dbContext.Category.Where(c => c.servicType_id == categoryid).FirstOrDefault();
        }

        public List<filter> AllFilters(int id)
        {
            return _dbContext.filter.Where(m=>m.serviceType_id == id && m.status == "Active").ToList();
        }

        public List<filter_value> FilterValues(int id)
        {
            return _dbContext.filter_value.Where(m => m.filter_id == id).ToList();
        }

        public filter_value ParticularFilterValue(int id)
        {
            return _dbContext.filter_value.Where(m => m.id == id).FirstOrDefault();
        }
    }
}
