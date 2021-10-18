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
    public class Scanners : BaseService, IScanners
    {
        private HttpClient client;

        public Scanners(HttpClient client)
        {
            this.client = client;
        }

        public async Task<APIWrapper<Scanner>> CreateScanner(Scanner scanner)
        {
            try
            {
                await client.PostAsJsonAsync<Scanner>("scanners", scanner);
                return Data(scanner);
            }
            catch (Exception e)
            {
                return Error(new Scanner(), "Failed to create scanner", e.Message);
            }
        }

        public async Task<APIWrapper<bool>> DeleteScanner(string id)
        {
            try
            {
                await client.DeleteAsync($"scanners/{id}");
                return Data(true);
            }
            catch (Exception e)
            {
                return Error(false, "Failed to delete scanner", e.Message);
            }
        }

        public async Task<APIWrapper<Scanner>> EditScanner(string id, Scanner scanner)
        {
            try
            {
                await client.PutAsJsonAsync<Scanner>($"scanners/{id}", scanner);
                return Data(scanner);

            }
            catch (Exception e)
            {
                return Error(new Scanner(), "Failed to edit scanner", e.Message);
            }
        }

        public async Task<APIWrapper<Scanner>> GetScanner(string id)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<Scanner>($"scanners/{id}"));
            }
            catch (Exception e)
            {

                return Error(new Scanner(), "Failed to get scanner from id", e.Message);
            }
        }

        public async Task<APIWrapper<List<Scanner>>> GetScanners()
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<Scanner>>($"scanners"));
            }
            catch (Exception e)
            {

                return Error(new List<Scanner>(), "Failed to get scanners", e.Message);
            }
        }
    }
}
