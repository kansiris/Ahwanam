using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Repository.db
{
   public class cartrepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<SPGETNpkg_Result> getvendorpkgs(string id)
        {
            return maaAahwanamEntities.SPGETNpkg(Convert.ToInt64(id)).ToList();
        }
        public List<GetCartItems_Result> CartItemList(int vid)
        {
            return maaAahwanamEntities.GetCartItems(vid).ToList();
        }
        
    }
}
