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

        public List<QuotationsList> GetAllQuotations()
        {
            return _dbContext.QuotationsList.ToList();
        }

        public int UpdateQuote(QuotationsList quotationsList)
        {
            var GetMasterRecord = _dbContext.QuotationsList.FirstOrDefault(m => m.Id == quotationsList.Id);
            GetMasterRecord.FirstTime = quotationsList.FirstTime;
            GetMasterRecord.FirstTimeQuoteDate = quotationsList.FirstTimeQuoteDate;
            GetMasterRecord.SecondTime = quotationsList.SecondTime;
            GetMasterRecord.SecondTimeQuoteDate = quotationsList.SecondTimeQuoteDate;
            GetMasterRecord.ThirdTime = quotationsList.ThirdTime;
            GetMasterRecord.ThirdTimeQuoteDate = quotationsList.ThirdTimeQuoteDate;
            GetMasterRecord.Status = "Quote Replied";
            _dbContext.Entry(GetMasterRecord).CurrentValues.SetValues(quotationsList);
            int count = _dbContext.SaveChanges();
            return count;
        }

        public int AddInstallments(QuoteResponse quoteResponse)
        {
            _dbContext.QuoteResponse.Add(quoteResponse);
            _dbContext.SaveChanges();
            return 1;
        }

        public List<QuoteResponse> GetAllQuoteResponses()
        {
            return _dbContext.QuoteResponse.ToList();
        }
    }
}
