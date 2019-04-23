using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class WhishListRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<AvailableWhishLists> GetUserWhishlist(string id)
        {
            return _dbContext.AvailableWhishLists.Where(m => m.UserID == id).ToList();

        }

        public AvailableWhishLists AddWhishList(AvailableWhishLists availableWhishLists)
        {
            _dbContext.AvailableWhishLists.Add(availableWhishLists);
            _dbContext.SaveChanges();
            return availableWhishLists;
        }

        public string RemoveWhishList(int WhishListID)
        {
            try
            {
                var list = _dbContext.AvailableWhishLists.FirstOrDefault(m => m.WhishListID == WhishListID);
                _dbContext.AvailableWhishLists.Remove(list);
                _dbContext.SaveChanges();
                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }

        public WishlistDetails AddUserwishlist(WishlistDetails wishlists)
        {
            _dbContext.WishlistDetails.Add(wishlists);
            _dbContext.SaveChanges();
            return wishlists;
        }

        public Userwishlistdetails AddUserwishlistitem(Userwishlistdetails userwishlists)
        {
            _dbContext.Userwishlistdetails.Add(userwishlists);
            _dbContext.SaveChanges();
            return userwishlists;
        }

        public WishlistDetails getwishlistdetails(long wishlistid)
        {
            return _dbContext.WishlistDetails.Where(w => w.WishlistdetailId == wishlistid).FirstOrDefault();
        }
        public GetwishlistDetails_Result Getwishlistdetail(long wishlistid)
        {
            return maaAahwanamEntities.GetwishlistDetails(wishlistid).FirstOrDefault();
        }

        public int Removewishlistitem(long vendorId, long vendorsubId, long UserId)
        {

            var getdata = _dbContext.Userwishlistdetails.Where(m => m.vendorId == vendorId && m.vendorsubId == vendorsubId && m.UserId == UserId).FirstOrDefault();
            _dbContext.Userwishlistdetails.Remove(getdata);
            return _dbContext.SaveChanges();

        }
         public int Removeitem(long vendorId,long wishlistid, long userid)
        {
            int i;
            var getdata = _dbContext.Userwishlistdetails.Where(m => m.vendorId == vendorId && m.wishlistId == wishlistid && m.UserId == userid).FirstOrDefault();
            if(getdata!=null)
            {
                _dbContext.Userwishlistdetails.Remove(getdata);
                _dbContext.SaveChanges();
                i = 1;
            }
            else
            {
                i = 0;
            }

            return i;
        }

        //public int Removewishlistitem(long vendorId, long vendorsubId, long UserId)
        //{
        //    var getdata = _dbContext.wishlist.Where(m => m.vendor == wishlistId).FirstOrDefault();
        //    _dbContext.wishlist.Remove(getdata);
        //    return _dbContext.SaveChanges();
        //}

        public long Getvendordetailsbyvendorid(long vendorid,long wishlistid)
        {
            var count = _dbContext.Userwishlistdetails.Where(v => v.vendorId == vendorid && v.wishlistId == wishlistid).FirstOrDefault();
            if (count != null)
                return count.vendorId;
            else
                //count.UserLoginId = 0;
                return 0;
        }

        public WishlistDetails Getuserfromwishlistbyuserid(long userid)
        {
            return _dbContext.WishlistDetails.Where(u => u.UserId == userid).FirstOrDefault();
        }

        //public Getdetailsofwishlistitem_Result Getdetailsofvendor(long vendorid)
        //{
        //    return maaAahwanamEntities.Getdetailsofwishlistitem(vendorid).FirstOrDefault();
        //}
        public Getwishlisdata_vendorid_Result Getdetailsofvendorbyid(long vendorid)
        {
            return maaAahwanamEntities.Getwishlisdata_vendorid(vendorid).FirstOrDefault();
        }

        public List<Getwishlistdetails_userid_Result> getwishlistdetailsbyuserid(long userid)
        {
            return maaAahwanamEntities.Getwishlistdetails_userid(userid).ToList();
        }

        public List<Getwishlistvendors_Result> getwishlistvendors(long wishlistid,int categoryid)
        {
            return maaAahwanamEntities.Getwishlistvendors(wishlistid, categoryid).ToList();
        }
        public List<Collabrator> Getcollabrators(long userid)
        {
            return _dbContext.Collabrator.Where(c => c.UserId == userid).ToList();
        }

        public List<Userwishlistdetails> getuserwishlistdata(long wishlistid)
        {
            return _dbContext.Userwishlistdetails.Where(w => w.wishlistId == wishlistid).ToList();
        }

        public List<sharedwishlist_Result> getsharedwishlist(string email)
        {
            return maaAahwanamEntities.sharedwishlist(email).ToList();
        }
    }
}
