﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class FilterServices
    {
        FiltersRepository filtersRepository = new FiltersRepository();
        public List<Category> AllCategories()
        {
            return filtersRepository.AllCategories();
        }

        public List<filter> AllFilters(int id)
        {
            return filtersRepository.AllFilters(id);
        }

        public List<filter_value> FilterValues(int id)
        {
            return filtersRepository.FilterValues(id);
        }
    }
}
