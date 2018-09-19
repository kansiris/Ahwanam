using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Service
{
  public class VendorDashBoardService
    {
        VendorDashBoardRepository mngvendorrepository = new VendorDashBoardRepository();
        public ManageVendor SaveVendor(ManageVendor mngvendor)
        {
            mngvendor = mngvendorrepository.AddVendor(mngvendor);
            return mngvendor;
        }
       public List<ManageVendor> getvendor(string Vid)
        {
            List<ManageVendor> mngvendorlist = new List<ManageVendor>();
            mngvendorlist = mngvendorrepository.GetVendorList(Vid);
            return mngvendorlist;
        }
        public ManageUser AddUser(ManageUser mnguser)
        {
            mnguser = mngvendorrepository.AddUser(mnguser);
            return mnguser;
        }
        public List<ManageUser> getuser(string Vid)
        {
            List<ManageUser> mnguserlist = new List<ManageUser>();
            mnguserlist = mngvendorrepository.GetUserList(Vid);
            return mnguserlist;
        }
    }
}
