using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class ContestsService
    {
        ContestsRepoitory contestsRepoitory = new ContestsRepoitory();

        public ContestMaster AddNewContest(ContestMaster contestMaster)
        {
            return contestsRepoitory.AddNewContest(contestMaster);
        }

        public List<ContestMaster> GetAllContests()
        {
            return contestsRepoitory.GetAllContests();
        }
    }
}
