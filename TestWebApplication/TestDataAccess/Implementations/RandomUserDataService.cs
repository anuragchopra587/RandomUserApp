using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestDataAccess.Contracts;
using TestDataAccess.Constants;
using Newtonsoft.Json;
using TestServiceModel;

namespace TestDataAccess.Implementations
{
    public class RandomUserDataService : IRandomUserDataService
    {
        public async Task<RandomUserApiResponse> GetRandomUserData()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Endpoints.RANDOM_USER_ENDPOINT);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<RandomUserApiResponse>(jsonString);
                    return userData;
                }
                else
                {
                    throw new Exception($"Failed to fetch random user data. Status code: {response.StatusCode}");
                }
            }
        }
    }

}
