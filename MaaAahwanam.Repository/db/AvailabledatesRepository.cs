﻿using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class AvailabledatesRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public Availabledates saveavailabledates(Availabledates availabledates)
        {
            availabledates= _dbContext.Availabledates.Add(availabledates);
            _dbContext.SaveChanges();
            return availabledates;
        }
    }
}
