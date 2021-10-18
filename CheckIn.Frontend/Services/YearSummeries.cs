using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    /// <inheritdoc/>
    public class YearSummeries : BaseService, IYearSummeries
    {
        private HttpClient client;

        public YearSummeries(HttpClient client)
        {
            this.client = client;
        }

        public async Task<APIWrapper<List<YearSummery>>> GetYearSummeries(string cardId, int year)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<YearSummery>>($"yearsummery/{cardId}/{year}"));
            }
            catch (Exception e)
            {

                return Error(new List<YearSummery>(), "Faild to get year summeries", e.Message);
            }
        }
    }
}
