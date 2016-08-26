using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class IssueDetailRepository
    {
        ApiContext _DBcontect = new ApiContext();
        public IssueDetail saveissuedetails(IssueDetail issueDetail)
        {
            _DBcontect.IssueDetail.Add(issueDetail);
            _DBcontect.SaveChanges();
            return issueDetail;
        }
        public List<sp_Tickets_Result> Getissuedetails(int ticketid)
        {
            MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
            List<sp_Tickets_Result> t1 = maaAahwanamEntities.sp_Tickets(ticketid).ToList();
            return t1;
        }
    }
}
