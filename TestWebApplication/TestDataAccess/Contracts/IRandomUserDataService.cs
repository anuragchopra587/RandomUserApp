using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestServiceModel;

namespace TestDataAccess.Contracts
{
    public interface IRandomUserDataService
    {
        Task<RandomUserApiResponse> GetRandomUserData();
    }
}
