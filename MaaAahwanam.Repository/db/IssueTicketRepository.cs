using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class IssueTicketRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<IssueTicket> IssueTicketsList(int UserId)
        {
            return _dbContext.IssueTicket.Where(a=>a.UserLoginId== UserId).ToList();
        }
        public int IssueTicketsListCounts()
        {
            return _dbContext.IssueTicket.Count();
        }
        public IssueTicket Insertissueticket(IssueTicket issueTicket)
        {
            _dbContext.IssueTicket.Add(issueTicket);
            _dbContext.SaveChanges();
            return issueTicket;
        }
    }
}
