using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class ResultsPageRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();

        public List<GetVendors_Result> GetAllVendors(string type)
        {
            return maaAahwanamEntities.GetVendors(type).ToList();
        }
    }
}
