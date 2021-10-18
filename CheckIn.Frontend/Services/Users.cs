using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    /// <inheritdoc/>
    public class Users : BaseService, IUsers
    {
        private readonly HttpClient client;

        public Users(IHttpClientFactory clientFactory)
        {
            this.client = clientFactory.CreateClient("skpauth");
        }

        public async Task<APIWrapper<bool>> AddCardToUser(string card, string userId)
        {
            try
            {
                await client.PatchAsync($"Users/{userId}", new StringContent("[{\"op\":\"replace\",\"path\":\"/CardId\",\"value\":\"" + card + "\"}]", Encoding.UTF8, "application/json-patch+json"));
                return Data(true);
            }
            catch (Exception e)
            {
                return Error(false, "Failed to add card to user", e.Message);
            }
        }

        public async Task<APIWrapper<PagedList<User>>> GetUsers(Pagination pagination)
        {
            try
            {
                var response = await client.GetAsync($"users{pagination.AsQuery}");
                var headerPage = JsonSerializer.Deserialize<Pagination>(response.Headers.GetValues("X-Pagination").First());
                pagination.currentPage = headerPage.currentPage;
                pagination.pageSize = headerPage.pageSize;
                pagination.totalCount = headerPage.totalCount;
                pagination.totalPages = headerPage.totalPages;
                return Data(new PagedList<User>
                {
                    Items = await response.Content.ReadFromJsonAsync<List<User>>(),
                    Pagination = pagination
                });

            }
            catch (Exception e)
            {
                return Error(new PagedList<User>(), "Faild to get users", e.Message);
            }
        }

        public async Task<APIWrapper<List<User>>> GetUsersFromCards(params string[] cards)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<User>>($"getusersfromcards?cards={string.Join("cards=", cards.Select(x => x += "&"))}"));
            }
            catch (Exception e)
            {
                return Error(new List<User>(), "Failed to get users from cards", e.Message);
            }
        }

        public async Task<APIWrapper<List<User>>> GetUsersFromCards(List<Card> cards)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<User>>($"getusersfromcards?cards={string.Join("cards=", cards.Select(x => x += "&"))}"));
            }
            catch (Exception e)
            {
                return Error(new List<User>(), "Failed to get users from cards", e.Message);
            }
        }
    }
}
