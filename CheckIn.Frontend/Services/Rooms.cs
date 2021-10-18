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
    public class Rooms : BaseService, IRooms
    {
        private HttpClient client;

        public Rooms(HttpClient client)
        {
            this.client = client;
        }

        public async Task<APIWrapper<Room>> CreateRoom(Room room)
        {
            try
            {
                await client.PostAsJsonAsync<Room>($"rooms", room);
                return Data(room);

            }
            catch (Exception e)
            {
                return Error(new Room(), "Failed to create room", e.Message);
            }
        }

        public async Task<APIWrapper<bool>> DeleteRoom(int id)
        {
            try
            {
                await client.DeleteAsync($"rooms/{id}");
                return Data(true);

            }
            catch (Exception e)
            {
                return Error(false, "Failed to delete room", e.Message);
            }
        }

        public async Task<APIWrapper<Room>> GetRoom(int id)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<Room>($"rooms/{id}"));

            }
            catch (Exception e)
            {
                return Error(new Room(), "Failed to get room from id", e.Message);
            }
        }

        public async Task<APIWrapper<List<Room>>> GetRooms()
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<Room>>("rooms"));

            }
            catch (Exception e)
            {
                return Error(new List<Room>(), "Failed to get rooms", e.Message);
            }
        }

        public async Task<APIWrapper<Room>> GetRoomsByCard(string cardID)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<Room>($"rooms/card/{cardID}"));

            }
            catch (Exception e)
            {
                return Error(new Room(), "Failed to get room from id", e.Message);
            }
        }

        public async Task<APIWrapper<List<Room>>> GetRoomsByDepartments(string[] dep)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<Room>>($"rooms/department?dep={string.Join("dep=", dep.Select(x => x += "&"))}"));

            }
            catch (Exception e)
            {
                return Error(new List<Room>(), "Failed to get rooms from departments", e.Message);
            }
        }

        public async Task<APIWrapper<Room>> MoveCardToRoom(int roomId, Card card)
        {
            try
            {
                await client.PostAsJsonAsync<Card>($"rooms/card/{roomId}", card);
                return Data(new Room());

            }
            catch (Exception e)
            {
                return Error(new Room(), "Failed to move card to room", e.Message);
            }
        }

        public async Task<APIWrapper<bool>> RemoveCardFromRoom(int roomId, string cardId)
        {
            try
            {
                await client.DeleteAsync($"rooms/card/{roomId}/{cardId}");
                return Data(true);

            }
            catch (Exception e)
            {
                return Error(false, "Failed to delete room", e.Message);
            }
        }

        public async Task<APIWrapper<Room>> UpadteRoom(int id, Room room)
        {
            try
            {
                await client.PutAsJsonAsync<Room>($"rooms/{id}", room);
                return Data(room);

            }
            catch (Exception e)
            {
                return Error(new Room(), "Failed to move card to room", e.Message);
            }
        }
    }
}
