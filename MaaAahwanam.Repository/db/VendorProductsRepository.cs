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

        public List<searchvendorproducts_Result> Getsearchvendorproducts_Result(string search,string type)
        {
            return maaAahwanamEntities.searchvendorproducts(search,type).ToList();
        }
        public List<vendorproducts_Result> Getvendorproducts_Result(string type)
        {
            return maaAahwanamEntities.vendorproducts(type).ToList();
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
            return maaAahwanamEntities.getalldeal().ToList();
        }
    }

}
