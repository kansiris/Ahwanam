using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;
namespace MaaAahwanam.Repository.db
{
    public class OthersRepository
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        //Comments Module
        public List<Comment> CommentsList()
        {
            return _dbContext.Comment.ToList();
        }

        public CommentDetail AddComment(CommentDetail commentDetail)
        {
            _dbContext.CommentDetail.Add(commentDetail);
            _dbContext.SaveChanges();
            return commentDetail;
        }

        public List<MaaAahwanam_Others_Comments_Result> CommentRecord(long id)
        {
            return maaAahwanamEntities.MaaAahwanam_Others_Comments(id).ToList();
        }

        public List<CommentDetail> CommentDetail(long id)
        {
            return _dbContext.CommentDetail.Where(m => m.CommentId == id).ToList();
        }

        //Tickets Module
        public List<IssueTicket> TicketList()
        {
            return _dbContext.IssueTicket.ToList();
        }
        public List<MaaAahwanam_Others_Tickets_Result> TicketRecord(long id)
        {
            return maaAahwanamEntities.MaaAahwanam_Others_Tickets(id).ToList();
        }
        public IssueDetail AddTicket(IssueDetail issueDetail)
        {
            _dbContext.IssueDetail.Add(issueDetail);
            _dbContext.SaveChanges();
            return issueDetail;
        }
        public List<IssueDetail> TicketDetail(long id)
        {
            return _dbContext.IssueDetail.Where(m => m.TicketId == id).ToList();
        }

        //Registered Users Module
        public List<MaaAahwanam_Others_RegisteredUsers_Result> RegisteredUsersList()
        {
            return maaAahwanamEntities.MaaAahwanam_Others_RegisteredUsers().ToList();
        }
        public List<MaaAahwanam_Others_RegisteredUsersDetails_Result> RegisteredUserDetails(long id)
        {
            return maaAahwanamEntities.MaaAahwanam_Others_RegisteredUsersDetails(id).ToList();
        }

        //Testimonals Module
        public List<MaaAahwanam_Others_Testimonials_Result> TestimonalsList()
        {
            return maaAahwanamEntities.MaaAahwanam_Others_Testimonials().ToList();
        }
        public List<MaaAahwanam_Others_TestimonialDetail_Result> TestimonalDetail(long id)
        {
            return maaAahwanamEntities.MaaAahwanam_Others_TestimonialDetail(id).ToList();
        }

        public List<MaaAahwanam_Others_AllRegisteredUsersDetails_Result> AllRegisteredUsersList()
        {
            return maaAahwanamEntities.MaaAahwanam_Others_AllRegisteredUsersDetails().ToList();
        }
        public AdminTestimonialPath AdminTestimonialPathStatus(long id,AdminTestimonialPath adminTestimonialPath)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);//DateTime.UtcNow.ToShortDateString();
            var Getadmintestionalpath = _dbContext.AdminTestimonialPath.Where(m=>m.Id == id).FirstOrDefault();
            adminTestimonialPath.Id = Getadmintestionalpath.Id;
            adminTestimonialPath.PathId = Getadmintestionalpath.PathId;
            adminTestimonialPath.ImagePath = Getadmintestionalpath.ImagePath;
            adminTestimonialPath.UpdatedBy = Getadmintestionalpath.UpdatedBy;
            adminTestimonialPath.UpdatedDate = updateddate;
            _dbContext.Entry(Getadmintestionalpath).CurrentValues.SetValues(adminTestimonialPath);
            _dbContext.SaveChanges();
            return adminTestimonialPath;
            
        }

        public AdminTestimonial AdminTestimonialStatus(long id, AdminTestimonial adminTestimonial)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);//DateTime.UtcNow.ToShortDateString();
            var Getadmintestional = _dbContext.AdminTesimonial.Where(m => m.Id == id).FirstOrDefault();
            adminTestimonial.Id = Getadmintestional.Id;
            adminTestimonial.Name = Getadmintestional.Name;
            adminTestimonial.Description = Getadmintestional.Description;
            adminTestimonial.Email = Getadmintestional.Email;
            adminTestimonial.UpdatedBy = Getadmintestional.UpdatedBy;
            adminTestimonial.UpdatedDate = updateddate;
            adminTestimonial.Orderid = Getadmintestional.Orderid;
            adminTestimonial.Ratings = Getadmintestional.Ratings;
            _dbContext.Entry(Getadmintestional).CurrentValues.SetValues(adminTestimonial);
            _dbContext.SaveChanges();
            return adminTestimonial;

        }

        public List<Notification> AllNotifications()
        {
            return _dbContext.Notification.Where(m => m.type == "Admin").ToList();
        }

        public List<allnotifications_Result> Notifications()
        {
            return maaAahwanamEntities.allnotifications().ToList();
        }
    }
}
