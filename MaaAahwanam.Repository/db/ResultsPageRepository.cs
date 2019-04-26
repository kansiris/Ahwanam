using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class ResultsPageRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();

        public List<GetVendors_Result> GetAllVendors(string type)
        {
            return maaAahwanamEntities.GetVendors(type).ToList();
        }
        public List<GetFilteredVendors_Result> GetVendorsByName(string type, string name)
        {
            return maaAahwanamEntities.GetFilteredVendors(type,name).ToList();
        }
        public Vendormaster GetVendorByEmail(string emailid)
        {
            return _dbContext.Vendormaster.Where(m => m.EmailId == emailid).FirstOrDefault();
        }
        public UserLogin GetUserLogdetails(UserLogin userLogin)
        {
            var data = _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName).FirstOrDefault(); // && p.UserType == userLogin.UserType
            return data;
        }
        public UserLogin GetUserLogin(UserLogin userLogin)
        {
            var data = _dbContext.UserLogin.Where(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password).FirstOrDefault(); // && p.UserType == userLogin.UserType
            return data;
        }

        public UserDetail GetUser(UserDetail userdetail)
        {
            var data = _dbContext.UserDetail.Where(u => u.UserDetailId == userdetail.UserDetailId && u.AlternativeEmailID == userdetail.AlternativeEmailID).FirstOrDefault();
            return data;
        }

        public List<GetPhotographers_Result> GetAllPhotographers()
        {
            return maaAahwanamEntities.GetPhotographers().ToList();
        }

        public List<GetDecorators_Result> GetAllDecorators()
        {
            return maaAahwanamEntities.GetDecorators().ToList();
        }

        public List<GetCaterers_Result> GetAllCaterers()
        {
            return maaAahwanamEntities.GetCaterers().ToList();
        }

        public List<GetOthers_Result> GetAllOthers(string type)
        {
            return maaAahwanamEntities.GetOthers(type).ToList();
        }

        public List<SP_Getvendormasterdata_Result> Getvendormasterdata()
        {
            return maaAahwanamEntities.SP_Getvendormasterdata().ToList();
        }

        public List<GetVendorsByCategoryId_Result> GetvendorbycategoryId(int CategoryTypeId)
        {
            return maaAahwanamEntities.GetVendorsByCategoryId(CategoryTypeId).ToList();
        }

        public List<GetCategoryByname_Result> Getvendorbycategory(string categorytype)
        {
            return maaAahwanamEntities.GetCategoryByname(categorytype).ToList();
        }

        public Vendormasterdata Getvendor(long vendorid)
        {
            var data = _dbContext.Vendormasterdata.Where(v=>v.VendormasterId == vendorid).FirstOrDefault();
            return data;
        }
        public Getvendor_vendorid_Result Getsupplier(long vendorid)
        {
            return maaAahwanamEntities.Getvendor_vendorid(vendorid).FirstOrDefault();
        }

        public List<VendorAmenity> GetAmenities(long vendorid)
        {
            return _dbContext.VendorAmenity.Where(a => a.VendorId == vendorid).ToList();
           
        }
        public List<VendorPolicies> GetPolicies(long vendorid)
        {
            return _dbContext.VendorPolicies.Where(p => p.VendorId == vendorid).ToList();
            
        }
        public List<VendorAvailableArea> GetavailableAreas(long vendorid)
        {
            return _dbContext.VendorAvailableArea.Where(v => v.VendorId == vendorid).ToList();
        }

        public List<Review> Getreviews(long vendorid)
        {
            return _dbContext.Review.Where(r => r.ServiceId == vendorid).ToList();
        }

        public List<VendormasterImage> Getimages(long vendorid)
        {
            return _dbContext.VendormasterImage.Where(i => i.VendorId == vendorid).ToList();
        }
        
    }
}
