using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendorVenueSignUpRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public UserLogin AddUserLogin(UserLogin userLogin)
        {
            _dbContext.UserLogin.Add(userLogin);
            _dbContext.SaveChanges();
            return userLogin;
        }

        public UserLogin GetUserLogin(UserLogin userLogin)
        {
           var data= _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password).FirstOrDefault(); // && p.UserType == userLogin.UserType
            return data;
        }

        public Vendormaster AddVendormaster(Vendormaster vendormaster)
        {
            _dbContext.Vendormaster.Add(vendormaster);
            _dbContext.SaveChanges();
            return vendormaster;
        }

        public UserDetail AddUserDetail(UserDetail userDetail)
        {
            _dbContext.UserDetail.Add(userDetail);
            _dbContext.SaveChanges();
            return userDetail;
        }

        public VendorVenue AddVendorVenue(VendorVenue vendorVenue)
        {
            _dbContext.VendorVenue.Add(vendorVenue);
            _dbContext.SaveChanges();
            return vendorVenue;
        }

        public VendorsCatering AddVendorCatering(VendorsCatering vendorsCatering)
        {
            _dbContext.VendorsCatering.Add(vendorsCatering);
            _dbContext.SaveChanges();
            return vendorsCatering;
        }
        public Package Addpackage(Package package)
        {
            _dbContext.Package.Add(package);
            _dbContext.SaveChanges();
            return package;
        }

        public NDeals Adddeals(NDeals deals)
        {
            _dbContext.NDeal.Add(deals);
            _dbContext.SaveChanges();
            return deals;
        }

        public List<VendorVenue> GetVendorVenue(long id)
        {
            return _dbContext.VendorVenue.Where(p => p.VendorMasterId == id).ToList();
        }

       

        public List<VendorsCatering> GetVendorCatering(long id)
        {
            return _dbContext.VendorsCatering.Where(p => p.VendorMasterId == id).ToList();
        }

        public VendorsPhotography AddVendorPhotography(VendorsPhotography vendorsPhotography)
        {
            _dbContext.VendorsPhotography.Add(vendorsPhotography);
            _dbContext.SaveChanges();
            return vendorsPhotography;
        }

        public List<VendorsPhotography> GetVendorPhotography(long id)
        {
            return _dbContext.VendorsPhotography.Where(p => p.VendorMasterId == id).ToList();
        }

        public VendorsDecorator AddVendorDecorator(VendorsDecorator vendorsDecorator)
        {
            _dbContext.VendorsDecorator.Add(vendorsDecorator);
            _dbContext.SaveChanges();
            return vendorsDecorator;
        }

        public List<VendorsDecorator> GetVendorDecorator(long id)
        {
            return _dbContext.VendorsDecorator.Where(p => p.VendorMasterId == id).ToList();
        }

        public List<VendorsEventOrganiser> GetVendorEventOrganiser(long id)
        {
            return _dbContext.VendorsEventOrganiser.Where(p => p.VendorMasterId == id).ToList();
        }

        public VendorsOther AddVendorOther(VendorsOther vendorsOther)
        {
            _dbContext.VendorsOther.Add(vendorsOther);
            _dbContext.SaveChanges();
            return vendorsOther;
        }

        public List<VendorsOther> GetVendorOther(long id)
        {
            return _dbContext.VendorsOther.Where(p => p.VendorMasterId == id).ToList();
        }

        public string RemoveVendorService(string id, string type)
        {
            long vid = long.Parse(id);
            try
            {
                if (type == "Venue")
                {
                    var list = _dbContext.VendorVenue.FirstOrDefault(m => m.Id == vid);
                    _dbContext.VendorVenue.Remove(list);
                }
                if (type == "Catering")
                {
                    var list = _dbContext.VendorsCatering.FirstOrDefault(m => m.Id == vid);
                    _dbContext.VendorsCatering.Remove(list);
                }
                if (type == "Photography")
                {
                    var list = _dbContext.VendorsPhotography.FirstOrDefault(m => m.Id == vid);
                    _dbContext.VendorsPhotography.Remove(list);
                }
                if (type == "Event")
                {
                    var list = _dbContext.VendorsEventOrganiser.FirstOrDefault(m => m.Id == vid);
                    _dbContext.VendorsEventOrganiser.Remove(list);
                }
                if (type == "Decorator")
                {
                    var list = _dbContext.VendorsDecorator.FirstOrDefault(m => m.Id == vid);
                    _dbContext.VendorsDecorator.Remove(list);
                }
                if (type == "Other")
                {
                    var list = _dbContext.VendorsOther.FirstOrDefault(m => m.Id == vid);
                    _dbContext.VendorsOther.Remove(list);
                }
                _dbContext.SaveChanges();
                return "Removed";
            }
            catch
            {
                return "Failed!!!";
            }
        }
    }
}
