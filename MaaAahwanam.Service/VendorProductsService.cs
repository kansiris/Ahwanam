﻿using System;
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
        public List<vendorproducts_Result> Getvendorproducts_Result(string type)
        {
            return vendorProductsRepository.Getvendorproducts_Result(type);
        }
    }
}