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
        public int checkvendoremail(string email, string id)
        {
            //int vendorsubid = int.Parse(id);
            // return mngvendorrepository.checkvendoremail(email,id);
            return mngvendorrepository.checkvendoremail(email, id);
        }
        public ManageVendor getvendorbyid(int id)
        {
            return mngvendorrepository.GetVendordetails(id);
        }

        public ManageVendor UpdateVendor(ManageVendor mngvendor, int id)
        {
            return mngvendorrepository.UpdateVendor(mngvendor, id);
        }
        public int checksupplierservices(string servicename, string Vid)
        {
            return mngvendorrepository.checksupplierservices(servicename, Vid);
        }
        public AllSupplierServices AddSupplierServices(AllSupplierServices supplierservices)
        {
            supplierservices = mngvendorrepository.AddSupplierServices(supplierservices);
            return supplierservices;
        }
        public List<AllSupplierServices> getsupplierservices(string vmid)
        {
            List<AllSupplierServices> supplierserviceslst = new List<AllSupplierServices>();
            supplierserviceslst = mngvendorrepository.GetSupplierServiceslst(vmid);
            return supplierserviceslst;
        }
        public AllSupplierServices getsuplierservicesbyid(long id)
        {
            return mngvendorrepository.GetSupplierService(id);
        }
        public AllSupplierServices updatesupplierservices(AllSupplierServices supplierservices,long id)
        {
            return mngvendorrepository.UpdateSupplierServices(supplierservices, id);
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
        public List<StaffAccess> getstaff(string Vid)
        {
            List<StaffAccess> stafflist = new List<StaffAccess>();
            stafflist = mngvendorrepository.GetstaffList(long.Parse(Vid));
            return stafflist;
        }
        public int checkuseremail(string email, string id)
        {

            return mngvendorrepository.checkuseremail(email, id);
        }
        public ManageUser getuserbyid(int id)
        {
            return mngvendorrepository.GetUserdetails(id);
        }
        public List<StaffAccess> getstaffbyid(int id)
        {
            return mngvendorrepository.getstaffbyid(id);
        }
        public List<ManageUser> getuserbyemail(string email)
        {
            return mngvendorrepository.getuserbyemail(email);
        }

        public ManageUser UpdateUser(ManageUser mnguser, int id)
        {
            return mngvendorrepository.UpdateUser(mnguser, id);
        }

        public List<PackageMenu> GetParticularMenu(string category, string id, string vid)
        {
            return mngvendorrepository.GetParticularMenu(category, id, vid);
        }

        public string UpdateMenuItems(PackageMenu packageMenu, string type)
        {
            return mngvendorrepository.UpdateMenuItems(packageMenu,type);
        }

        public int AddVegMenu(PackageMenu packageMenu)
        {
            return mngvendorrepository.AddVegMenu(packageMenu);
        }

        public int AddNonVegMenu(PackageMenu packageMenu)
        {
            return mngvendorrepository.AddNonVegMenu(packageMenu);
        }
    }
}
