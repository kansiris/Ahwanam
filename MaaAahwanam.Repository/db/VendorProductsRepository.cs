using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendorProductsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();

        public List<filtervendors_Result> filtervendors_Result(string type,string f1, string f2, string f3)
        {
            return maaAahwanamEntities.filtervendors(type, f1, f2,f3).ToList();
        }

        public List<searchvendorproducts_Result> Getsearchvendorproducts_Result(string search,string type)
        {
            return maaAahwanamEntities.searchvendorproducts(search,type).ToList();
        }
        public List<vendorproducts_Result> Getvendorproducts_Result(string type)
        {
            return maaAahwanamEntities.vendorproducts(type).ToList();
        }

        public List<addvendorservices_Result> getvendorsubid(string id)
        {
            return maaAahwanamEntities.addvendorservices(Convert.ToInt64(id)).ToList();
        }
        public List<searchvendors_Result> GetSearchedVendorRecords(string type,string param)
        {
            return maaAahwanamEntities.searchvendors(type,param).ToList();
        }
        public List<spsearchword_Result> spsearchword(string search, string Type)
        {
            return maaAahwanamEntities.spsearchword(search, Type).ToList();
        }
        public List<spsearchdeal_Result> spsearchdeal(string search, string Type)
        {
            return maaAahwanamEntities.spsearchdeal(search, Type).ToList();
        }
        public List<spsearchdealname_Result> spsearchdealname(string name, string Type)
        {
            return maaAahwanamEntities.spsearchdealname(name, Type).ToList();
        }

        public List<Spgetalldeals_Result> getalldeal()
        {
            return maaAahwanamEntities.Spgetalldeals().ToList();
        }
        public List<Spgetalleventdeals_Result> getalleventdeal(string eve)
        {
            return maaAahwanamEntities.Spgetalleventdeals(eve).ToList();
        }
        public List<Spalldeals_Result> getparticulardeal(int id, string type)
        {
            return maaAahwanamEntities.Spalldeals(id, type).ToList();
        }
        public List<speventvdeals_Result> getpartvendordeal(string id, string type)
        {
            return maaAahwanamEntities.speventvdeals(id, type).ToList();
        }
    }

}
