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
    public class FilterServices
    {
        FiltersRepository filtersRepository = new FiltersRepository();
        public List<Category> AllCategories()
        {
            return filtersRepository.AllCategories();
        }

        public Category category(int categoryid)
        {
            
            return filtersRepository.category(categoryid);
        }

        public categoriesbycid_Result getcategory(int categoryid)
        {
            return filtersRepository.getcategory(categoryid);
        }


        public List<filter> AllFilters(int id)
        {
            return filtersRepository.AllFilters(id);
        }

        public List<filter_value> FilterValues(int id)
        {
            return filtersRepository.FilterValues(id);
        }

        public List<newfilterresult> newFilterValues(int id)
        {
            return filtersRepository.newFilterValues(id);
        }
        public filter_value ParticularFilterValue(int id)
        {
            return filtersRepository.ParticularFilterValue(id);
        }

        public newfilterresult newfiltervalue(int id)
        {
            return filtersRepository.newfiltervalue(id);
        }


    }
}
