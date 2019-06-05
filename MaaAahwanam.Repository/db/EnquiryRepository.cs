using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class EnquiryRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public Enquiry SaveEnquiries(Enquiry enquiry)
        {
            _dbContext.Enquiry.Add(enquiry);
            _dbContext.SaveChanges();
            return enquiry;
        }

        public List<Enquiry> getallenquires()
        {
            return _dbContext.Enquiry.OrderByDescending(a => a.EnquiryDate).ToList();
        }

        public List<getallwishlistdetailsofusers_Result> Getwishlistdataforadmin()
        {
            return maaAahwanamEntities.getallwishlistdetailsofusers().OrderByDescending(w => w.WishlistdetailId).ToList();
        }

        public List<getuserdetailsforadmin_Result> Getuserdataforadmin()
        {
            return maaAahwanamEntities.getuserdetailsforadmin().OrderByDescending(u => u.UserDetailId).ToList();
        }

    }
}
