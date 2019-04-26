using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class ResultsPageService
    {
        ResultsPageRepository resultsPageRepository = new ResultsPageRepository();

        public List<GetVendors_Result> GetAllVendors(string type)
        {
            return resultsPageRepository.GetAllVendors(type);
        }
        public List<GetFilteredVendors_Result> GetVendorsByName(string type, string name)
        {
            return resultsPageRepository.GetVendorsByName(type, name);
        }
        public Vendormaster GetVendorByEmail(string emailid)
        {
            return resultsPageRepository.GetVendorByEmail(emailid);
        }
        public UserLogin GetUserLogdetails(UserLogin userLogin)
        {
            return resultsPageRepository.GetUserLogdetails(userLogin);
        }
        public UserLogin GetUserLogin(UserLogin userLogin)
        {
            return resultsPageRepository.GetUserLogin(userLogin);
        }

        public List<GetPhotographers_Result> GetAllPhotographers()
        {
            return resultsPageRepository.GetAllPhotographers();
        }

        public List<GetDecorators_Result> GetAllDecorators()
        {
            return resultsPageRepository.GetAllDecorators();
        }

        public List<GetCaterers_Result> GetAllCaterers()
        {
            return resultsPageRepository.GetAllCaterers();
        }

        public List<GetOthers_Result> GetAllOthers(string type)
        {
            return resultsPageRepository.GetAllOthers(type);
        }

        public UserDetail GetUser(UserDetail userdetail)
        {
            return resultsPageRepository.GetUser(userdetail);
        }

        public List<SP_Getvendormasterdata_Result> Getvendormasterdata()
        {
            return resultsPageRepository.Getvendormasterdata();
        }

        public List<GetVendorsByCategoryId_Result> GetvendorbycategoryId(int CategoryTypeId)
        {
            return resultsPageRepository.GetvendorbycategoryId(CategoryTypeId);
        }
        public List<GetCategoryByname_Result> Getvendorbycategory(string categorytype)
        {
            return resultsPageRepository.Getvendorbycategory(categorytype);
        }

        public Vendormasterdata Getvendor(long vendorid)
        {
            return resultsPageRepository.Getvendor(vendorid);
        }

        public List<VendorAmenity> GetAmenities(long vendorid)
        {
            return resultsPageRepository.GetAmenities(vendorid);
        }

        public List<VendorPolicies> GetPolicies(long vendorid)
        {
            return resultsPageRepository.GetPolicies(vendorid);
        }
        public Getvendor_vendorid_Result Getsupplier(long vendorid)
        {
            return resultsPageRepository.Getsupplier(vendorid);
        }

        public List<VendorAvailableArea> GetavailableAreas(long vendorid)
        {
            return resultsPageRepository.GetavailableAreas(vendorid);
        }

        public List<Review> Getreviews(long vendorid)
        {
            return resultsPageRepository.Getreviews(vendorid);
        }

        public List<VendormasterImage> Getimages(long vendorid)
        {
            return resultsPageRepository.Getimages(vendorid);
        }

        public List<allcategories_Result> getallcategories()
        {
            return resultsPageRepository.getallcategories();
        }
    }
}
