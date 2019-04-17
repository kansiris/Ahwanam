using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class WhishListService
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        WhishListRepository whishListRepository = new WhishListRepository();
        NotesRepository noterepo = new NotesRepository();

        DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        public AvailableWhishLists AddWhishList(AvailableWhishLists availableWhishLists)
        {
            availableWhishLists.Status = "InActive";
            availableWhishLists.WhishListedDate = updateddate;
            return whishListRepository.AddWhishList(availableWhishLists);
        }

        public List<AvailableWhishLists> GetWhishList(string id)
        {
            return whishListRepository.GetUserWhishlist(id);
        }

        public string RemoveWhishList(int WhishListID)
        {
            return whishListRepository.RemoveWhishList(WhishListID);
        }

        public string AddAllwishlist(WishlistDetails wishlists, Userwishlistdetails userwishlists)
        {
            string response;
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            wishlists.UpdatedDate = userwishlists.WhishListedDate = updateddate;
            try
            {
                WishlistDetails w1 = whishListRepository.AddUserwishlist(wishlists);
                //userwishlists.wishlistId = w1.wishlistId;
                userwishlists.UserId = w1.UserId;
                Userwishlistdetails w2 = whishListRepository.AddUserwishlistitem(userwishlists);
                response = "success";
            }
            catch (Exception ex)
            {
                response = "failure";
            }
            return response;
        }

        public WishlistDetails AddUserwishlist(WishlistDetails wishlists)
        {
            return whishListRepository.AddUserwishlist(wishlists);
        }

        public Userwishlistdetails Adduserwishlistitem(Userwishlistdetails userwishlists)
        {
            return whishListRepository.AddUserwishlistitem(userwishlists);
        }

        public WishlistDetails getwishlistdetails(long wishlistid)
        {
            return whishListRepository.getwishlistdetails(wishlistid);
        }

        public GetwishlistDetails_Result Getwishlistdetail(long wishlistid)
        {
            return whishListRepository.Getwishlistdetail(wishlistid);
        }
        public int Removewishlistitem(long vendorId, long vendorsubId, long UserId)
        {
            return whishListRepository.Removewishlistitem(vendorId, vendorsubId, UserId);
        }

        public int Removeitem(long vendorId, long wishlistid, long userid)
        {
            return whishListRepository.Removeitem(vendorId, wishlistid, userid);
        }
        //public int Removewishlistitem(long wishlistId)
        //{
        //    return whishListRepository.Removewishlistitem(wishlistId);
        //}

        public List<Note> Getnote(long wishlist_id, long vendor_id)
        {
            return noterepo.Getnote(wishlist_id, vendor_id);
        }
        public Note AddNotes(Note note)
        {
            return noterepo.AddNotes(note);
        }

        public Note UpdateNotes(Note n1, long notesId)
        {
            return noterepo.UpdateNotes(n1, notesId);
        }
       

        public int RemoveNotes(long notesId)
        {
            return noterepo.RemoveNotes(notesId);
        }

        public collabratornotes addcollabratornote(collabratornotes cnotes)
        {
            return noterepo.addcollabratornote(cnotes);
        }

        public collabratornotes UpdatecollabratorNotes(collabratornotes note, long notesId)
        {
            return noterepo.UpdatecollabratorNotes(note, notesId);
        }
        public int RemovecollabratorNotes(long notesId)
        {
            return noterepo.RemovecollabratorNotes(notesId);
        }

        public long GetcollabratorDetailsByEmail(string username)
        {
            return noterepo.GetcollabratorDetailsByEmail(username);
        }

        public Collabrator AddCollabrator(Collabrator collabrator)
        {
                return noterepo.AddCollabrator(collabrator);
        }
        public int RemoveCollabrator(long collabratorId)
        {
            return noterepo.RemoveCollabrator(collabratorId);
        }
        public long Getvendordetailsbyvendorid(long vendorid, long wishlistid)
        {
            return whishListRepository.Getvendordetailsbyvendorid(vendorid, wishlistid);
        }
        public WishlistDetails Getuserfromwishlistbyuserid(long userid)
        {
            return whishListRepository.Getuserfromwishlistbyuserid(userid);
        }
        //public Getdetailsofwishlistitem_Result Getdetailsofvendor(long vendorid)
        //{
        //    return whishListRepository.Getdetailsofvendor(vendorid);
        //}

        public Getwishlisdata_vendorid_Result Getdetailsofvendorbyid(long vendorid)
        {
            return whishListRepository.Getdetailsofvendorbyid(vendorid);
        }
        public List<Getwishlistdetails_userid_Result> getwishlistdetailsbyuserid(long userid)
        {
            return whishListRepository.getwishlistdetailsbyuserid(userid);
        }
        public List<Getwishlistvendors_Result> getwishlistvendors(long wishlistid, int categoryid)
        {
            return whishListRepository.getwishlistvendors(wishlistid, categoryid);
        }

        public List<Collabrator> Getcollabrators(long userid)
        {
            return whishListRepository.Getcollabrators(userid);
        }

        public List<Userwishlistdetails> getuserwishlistdata(long wishlistid)
        {
            return whishListRepository.getuserwishlistdata(wishlistid);
        }
    }
}
