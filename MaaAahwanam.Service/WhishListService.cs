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
        WhishListRepository whishListRepository = new WhishListRepository();

        string updateddate = DateTime.UtcNow.ToShortDateString();

        public AvailableWhishLists AddWhishList(AvailableWhishLists availableWhishLists)
        {
            availableWhishLists.Status = "InActive";
            availableWhishLists.WhishListedDate = Convert.ToDateTime(updateddate);
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
    }
}
