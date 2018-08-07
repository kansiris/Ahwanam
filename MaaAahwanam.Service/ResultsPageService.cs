using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository;
using MaaAahwanam.Repository.db;

namespace MaaAahwanam.Service
{
    public class ResultsPageService
    {
        ResultsPageRepository resultsPageRepository = new ResultsPageRepository();

        public List<GetVendors_Result> GetAllVendors(string type)
        {
            return resultsPageRepository.GetAllVendors(type);
        }
    }
}
