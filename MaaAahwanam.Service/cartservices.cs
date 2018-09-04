using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository;
using MaaAahwanam.Repository.db;

namespace MaaAahwanam.Service
{
  public  class cartservices
    {
        cartrepository cartres = new cartrepository();
        public List<SPGETNpkg_Result> getvendorpkgs(string id)
        {
            return cartres.getvendorpkgs(id);
        }
        public int CartItemsCount(int UserId)
        {
            var l1 = 0;
            if (UserId != 0)
                l1 = cartres.CartItemList(UserId).Count();
            return l1;
        }
        public List<GetCartItems_Result> CartItemsList(int vid)
        {
            List<GetCartItems_Result> l1 = cartres.CartItemList(vid);
            return l1;
        }
        public int CartItemsCount1(int UserId)
        {
            var l1 = 0;
            if (UserId != 0)
                l1 = (cartres.CartItemList(UserId).Where(m => m.Status == "Active")).Count();
            return l1;
        }
        public List<GetCartItems_Result> CartItemsList1(int vid)
        {
            return cartres.CartItemList(vid).Where((i => i.Status == "Active")).ToList();
        }
    }
}
