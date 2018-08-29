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
    }
}
