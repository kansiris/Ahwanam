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

        public wishlist Addwishlist(wishlist wishlists)
        {
            _dbContext.wishlist.Add(wishlists);
            _dbContext.SaveChanges();
            return wishlists;
        }

        public Userwishlist AddUserwishlist(Userwishlist userwishlists)
        {
            _dbContext.Userwishlist.Add(userwishlists);
            _dbContext.SaveChanges();
            return userwishlists;
        }

        public int RemoveuserWishList(long wishlistId)
        {
            var getdata = _dbContext.Userwishlist.Where(m => m.wishlistId == wishlistId).FirstOrDefault();
            _dbContext.Userwishlist.Remove(getdata);
            return _dbContext.SaveChanges();
        }

        public int Removewishlists(long wishlistId)
        {
            var data = _dbContext.wishlist.Where(m => m.Id == wishlistId).FirstOrDefault();
            _dbContext.wishlist.Remove(data);
            return _dbContext.SaveChanges();
        }
    }
}
