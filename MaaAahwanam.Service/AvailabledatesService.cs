using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;

namespace MaaAahwanam.Service
{
    public class AvailabledatesService
    {
        AvailabledatesRepository availabledatesRepository = new AvailabledatesRepository();
        public string saveavailabledates(Availabledates availabledates)
        {
            string message = "";
            availabledates=availabledatesRepository.saveavailabledates(availabledates);
            if (availabledates != null)
            {
                if (availabledates.Id != 0)
                {
                    message = "Success";
                }
                else
                {
                    message = "failed";
                }
            }
            else
            {
                message = "Failed";
            }
            return message;
        }

        public List<Availabledates> GetDates(long id)
        {
            return availabledatesRepository.GetDates(id);
        }
    }
}
