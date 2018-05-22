using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository;
using MaaAahwanam.Repository.db;

namespace MaaAahwanam.Service
{
    public class VendorProductsService
    {
        VendorProductsRepository vendorProductsRepository = new VendorProductsRepository();

        public List<filtervendors_Result> Getfiltervendors_Result(string type, string f1, string f2, string f3)
        {
            return vendorProductsRepository.filtervendors_Result(type, f1, f2, f3);
        }
        public List<addvendorservices_Result> getvendorsubid(string id)
        {
            return vendorProductsRepository.getvendorsubid(id);
        }
        public List<SPGETNpkg_Result> getvendorpkgs(string id)
        {
            return vendorProductsRepository.getvendorpkgs(id);
        }
        public List<SPGETpartpkg_Result> getpartpkgs(string id)
        {
            return vendorProductsRepository.getpartpkgs(Convert.ToInt64(id));
        }
        public List<SPGETNDeal_Result> getvendordeals(string id)
        {
            return vendorProductsRepository.getvendordeals(id);
        }
        public List<SPgetpartdeal_Result> getpartdeals(string id)
        {
            return vendorProductsRepository.getpartdeals(id);
        }
        public List<searchvendorproducts_Result> Getsearchvendorproducts_Result(string search,string type)
        {
            return vendorProductsRepository.Getsearchvendorproducts_Result(search,type);
        }
        public List<vendorproducts_Result> Getvendorproducts_Result(string type)
        {
            return vendorProductsRepository.Getvendorproducts_Result(type);
        }

        public List<searchvendors_Result> GetSearchedVendorRecords(string type,string param)
        {
            return vendorProductsRepository.GetSearchedVendorRecords(type,param);
        }
        public List<spsearchword_Result> getwordsearch(string search, string type)
        {
            return vendorProductsRepository.spsearchword(search, type);
        }
        public List<spsearchdeal_Result> getdealsearch(string search, string type)
        {
            return vendorProductsRepository.spsearchdeal(search, type);
        }
        public List<spsearchdealname_Result> getdealname(string name, string type)
        {
            return vendorProductsRepository.spsearchdealname(name, type);
        }
        public List<Spgetalldeals_Result> getalldeal()
        {
            return vendorProductsRepository.getalldeal();
        }
        public List<Spgetalleventdeals_Result> getalleventdeal(string eve, DateTime date)
        {
            return vendorProductsRepository.getalleventdeal(eve,date);
        }
        public List<Spalldeals_Result> getparticulardeal(int id,  string type)
        {
            return vendorProductsRepository.getparticulardeal(id,  type);
        }
        public List<speventvdeals_Result> getpartvendordeal(string id, string type,DateTime date)
        {
            return vendorProductsRepository.getpartvendordeal(id, type,  date);
        }
    }
}
