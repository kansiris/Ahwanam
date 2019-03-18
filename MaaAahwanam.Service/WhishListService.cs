using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;

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

        public wishlist Addwishlist(wishlist wishlists)
        {
            return whishListRepository.Addwishlist(wishlists);
        }

        public Userwishlist Adduserwishlist(Userwishlist userwishlists)
        {
            return whishListRepository.AddUserwishlist(userwishlists);
        }

        public wishlist getwishlist(long userId)
        {
            return whishListRepository.getwishlist(userId);
        }

        public int RemoveUserWishlist(long wishlistId)
        {
            return whishListRepository.RemoveuserWishList(wishlistId);
        }

        public int Removewishlist(long wishlistId)
        {
            return whishListRepository.Removewishlists(wishlistId);
        }
        public Note AddNotes(Note note)
        {
            return noterepo.AddNotes(note);
        }

        public Note UpdateNotes(Note note, long notesId)
        {
            return noterepo.UpdateNotes(note, notesId);
        }

        public int RemoveNotes(long notesId)
        {
            return noterepo.RemoveNotes(notesId);
        }

        public Collabrator AddCollabrator(Collabrator collabrator)
        {
            return noterepo.AddCollabrator(collabrator);
        }
        public int RemoveCollabrator(long collabratorId)
        {
            return noterepo.RemoveCollabrator(collabratorId);
        }
    }
}
