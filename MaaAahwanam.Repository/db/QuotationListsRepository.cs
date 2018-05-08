using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class QuotationListsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public int AddQuotationList(QuotationsList quotationsList)
        {
            _dbContext.QuotationsList.Add(quotationsList);
            _dbContext.SaveChanges();
            return 1;
        }

        public List<QuotationsList> GetQuotationsList(string IP)
        {
            return _dbContext.QuotationsList.Where(m => m.IPaddress == IP).ToList();
        }
    }
}
