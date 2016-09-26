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
    public class OthersService
    {
        readonly ApiContext _dbContext = new ApiContext();
        OthersRepository othersRepository = new OthersRepository();
        //Comments Module
        public List<Comment> CommentList()
        {
            return othersRepository.CommentsList();
        }

        public CommentDetail AddComment(CommentDetail commentDetail)
        {
            commentDetail.Status = "Active";
            commentDetail.UpdatedDate = DateTime.Now;
            return othersRepository.AddComment(commentDetail);
        }

        public List<MaaAahwanam_Others_Comments_Result> CommentRecordService(long id)
        {
            return othersRepository.CommentRecord(id);
        }

        public List<CommentDetail> CommentDetail(long id)
        {
            return othersRepository.CommentDetail(id);
        }

        //Tickets Module
        public List<IssueTicket> TicketList()
        {
            return othersRepository.TicketList();
        }
        public List<MaaAahwanam_Others_Tickets_Result> TicketRecordService(long id)
        {
            return othersRepository.TicketRecord(id);
        }
        public IssueDetail AddTicket(IssueDetail issueDetail)
        {
            issueDetail.Status = "Active";
            issueDetail.UpdatedDate = DateTime.Now;
            return othersRepository.AddTicket(issueDetail);
        }
        public List<IssueDetail> TicketDetail(long id)
        {
            return othersRepository.TicketDetail(id);
        }

        //Registered Users Module
        public List<MaaAahwanam_Others_RegisteredUsers_Result> RegisteredUsersList()
        {
            return othersRepository.RegisteredUsersList();
        }
        public List<MaaAahwanam_Others_RegisteredUsersDetails_Result> RegisteredUsersDetails(long id)
        {
            return othersRepository.RegisteredUserDetails(id);
        }

        public List<MaaAahwanam_Others_AllRegisteredUsersDetails_Result> AllRegisteredUsersDetails()
        {
            return othersRepository.AllRegisteredUsersList();
        }

        //Testimonals Module
        public List<MaaAahwanam_Others_Testimonials_Result> TestimonalsList()
        {
            return othersRepository.TestimonalsList();
        }
        public List<MaaAahwanam_Others_TestimonialDetail_Result> TestimonalDetail(long id)
        {
            return othersRepository.TestimonalDetail(id);
        }

        public AdminTestimonialPath AdminTestimonialPathStatus(long id, AdminTestimonialPath adminTestimonialPath)
        {
            return othersRepository.AdminTestimonialPathStatus(id,adminTestimonialPath);

        }

        public AdminTestimonial AdminTestimonialStatus(long id, AdminTestimonial adminTestimonial)
        {
            return othersRepository.AdminTestimonialStatus(id, adminTestimonial);

        }

        public List<Notification> AllNotifications()
        {
            return othersRepository.AllNotifications();
        }

        public List<allnotifications_Result> Notifications()
        {
            return othersRepository.Notifications();
        }
    }
}
