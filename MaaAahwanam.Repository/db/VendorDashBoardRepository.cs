using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class VendorDashBoardRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public ManageVendor AddVendor(ManageVendor mngvendor)
        {
            _dbContext.ManageVendor.Add(mngvendor);
            _dbContext.SaveChanges();
            return mngvendor;
        }
        public List<ManageVendor> GetVendorList(string Vid)
        {
            return _dbContext.ManageVendor.Where(v => v.vendorId == Vid).ToList();
        }
        //public int GetSubVendorId(string Vid)
        //{
        //    var query = from Vendorsubid in _dbContext.ManageVendor where ManageVendor.vendorId == Vid select ManageVendor.id;

        //}
        public ManageVendor UpdateVendor(ManageVendor vendor, int id)
        {
            var GetVendor = _dbContext.ManageVendor.SingleOrDefault(v => v.id == id);
            vendor.id = GetVendor.id;
            vendor.vendorId = GetVendor.vendorId;
            vendor.registereddate = GetVendor.registereddate;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendor);
            _dbContext.SaveChanges();
            return GetVendor;
        }
        public ManageVendor GetVendordetails(int id)
        {
            //var query = from vendor in _dbContext.ManageVendor where vendor.id == id select vendor;
            return _dbContext.ManageVendor.Where(v => v.id == id).FirstOrDefault();

        }
        public int checkvendoremail(string email, string id)
        {
            int c = _dbContext.ManageVendor.Where(e => e.email == email && e.vendorId == id).Count();
            //int count = _dbContext.ManageVendor.Where(e => e.email == email && e.id == int.Parse(id)).Count();
            return c;
        }
        public AllSupplierServices AddSupplierServices(AllSupplierServices supplierservices)
        {
            _dbContext.AllSupplierServices.Add(supplierservices);
            _dbContext.SaveChanges();
            return supplierservices;
        }
        public AllSupplierServices UpdateSupplierServices(AllSupplierServices supplierservices,long ID)
        {
            var getsupplierservices = _dbContext.AllSupplierServices.SingleOrDefault(s => s.ID == ID);
            supplierservices.ID = getsupplierservices.ID;
            supplierservices.VendorMasterID = getsupplierservices.VendorMasterID;
            supplierservices.Status = getsupplierservices.Status;
            _dbContext.Entry(getsupplierservices).CurrentValues.SetValues(supplierservices);
            _dbContext.SaveChanges();
            return getsupplierservices;
        }
        public AllSupplierServices GetSupplierService(long id)
        {
            return _dbContext.AllSupplierServices.Where(s => s.ID == id).FirstOrDefault();
        }
        public List<AllSupplierServices> GetSupplierServiceslst(string VmId)
        {
            //var servicesquery = (from S in _dbContext.AllSupplierServices where S.VendorMasterID == VmId select S.ServiceName).Distinct().ToList();
            var servicesquery= _dbContext.AllSupplierServices.Where(S => S.VendorMasterID == VmId).Distinct().ToList();
            var S1 = servicesquery;//.GroupBy(s => s.ServiceName).Select(i => i.First()).ToList();
            return S1;
        }
        public int checksupplierservices(string servicename, string Vid)
        {
            int SupplierServiceCount = _dbContext.AllSupplierServices.Where(S => S.ServiceName == servicename && S.VendorMasterID == Vid).Count();
            return SupplierServiceCount;
        }
        public ManageUser AddUser(ManageUser mnguser)
        {
            _dbContext.ManageUser.Add(mnguser);
            _dbContext.SaveChanges();
            return mnguser;

        }
        public List<ManageUser> GetUserList(string Vid)
        {
            return _dbContext.ManageUser.Where(v => v.vendorId == Vid).ToList();
        }
        public List<StaffAccess> GetstaffList(long Vid)
        {
            return _dbContext.StaffAccess.Where(v => v.VendorMasterID == Vid).ToList();
        }

        public int checkuseremail(string email, string id)
        {
            int UseremailCount = _dbContext.ManageUser.Where(e => e.email == email && e.vendorId == id).Count();
            //int count = _dbContext.ManageUser.Where(e => e.email == email && e.id == int.Parse(id)).Count();
            return UseremailCount;
        }
        public List<ManageUser> getuserbyemail(string email)
        {
            return _dbContext.ManageUser.Where(e => e.email == email).ToList();

        }
        public ManageUser UpdateUser(ManageUser User, int id)
        {
            var GetUser = _dbContext.ManageUser.SingleOrDefault(v => v.id == id);
            User.id = GetUser.id;
            User.vendorId = GetUser.vendorId;
            User.registereddate = GetUser.registereddate;
            _dbContext.Entry(GetUser).CurrentValues.SetValues(User);
            _dbContext.SaveChanges();
            return GetUser;
        }

        public ManageUser GetUserdetails(int id)
        {
            //var query = from vendor in _dbContext.ManageVendor where vendor.id == id select vendor;
            return _dbContext.ManageUser.Where(v => v.id == id).FirstOrDefault();

        }
        public List<StaffAccess> getstaffbyid(int id)
        {
            //var query = from vendor in _dbContext.ManageVendor where vendor.id == id select vendor;
            return _dbContext.StaffAccess.Where(v => v.ID == id).ToList();

        }

        public List<PackageMenu> GetParticularMenu(string category, string id, string vid)
        {
            return _dbContext.PackageMenu.Where(m => m.VendorMasterID == id && m.VendorID == vid && m.Category == category).ToList();
        }

        public string UpdateMenuItems(PackageMenu packageMenu, string type)
        {
            var GetItem = _dbContext.PackageMenu.FirstOrDefault(m => m.VendorMasterID == packageMenu.VendorMasterID && m.VendorID == packageMenu.VendorID && m.Category == packageMenu.Category);
            if (type != "Welcome Drinks") packageMenu.Welcome_Drinks = GetItem.Welcome_Drinks;
            if (type != "Starters") packageMenu.Starters = GetItem.Starters;
            if (type != "Rice") packageMenu.Rice = GetItem.Rice;
            if (type != "Bread") packageMenu.Bread = GetItem.Bread;
            if (type != "Curries") packageMenu.Curries = GetItem.Curries;
            if (type != "Fry/Dry") packageMenu.Fry_Dry = GetItem.Fry_Dry;
            if (type != "Salads") packageMenu.Salads = GetItem.Salads;
            if (type != "Soups") packageMenu.Soups = GetItem.Soups;
            if (type != "Deserts") packageMenu.Deserts = GetItem.Deserts;
            if (type != "Beverages") packageMenu.Beverages = GetItem.Beverages;
            if (type != "Fruits") packageMenu.Fruits = GetItem.Fruits;
            packageMenu.MenuID = GetItem.MenuID;
            _dbContext.Entry(GetItem).CurrentValues.SetValues(packageMenu);
            _dbContext.SaveChanges();
            return "Updated";
        }

        public int AddVegMenu(PackageMenu packageMenu)
        {
            _dbContext.PackageMenu.Add(packageMenu);
            int count = _dbContext.SaveChanges();
            return count;
        }

        public int AddNonVegMenu(PackageMenu packageMenu)
        {
            _dbContext.PackageMenu.Add(packageMenu);
            int count = _dbContext.SaveChanges();
            return count;
        }
    }
}
