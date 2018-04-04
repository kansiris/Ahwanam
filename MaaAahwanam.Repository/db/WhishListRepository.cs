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
    }
}
