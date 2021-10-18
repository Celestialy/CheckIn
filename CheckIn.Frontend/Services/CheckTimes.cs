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
    public class CheckTimes : BaseService, ICheckTimes
    {
        private HttpClient client;

        public CheckTimes(HttpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Creates a new checktime
        /// </summary>
        /// <param name="checkTime">CheckTime to create</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<CheckTime>> CreateChecktime(CheckTime checkTime)
        {
            try
            {
                await client.PostAsJsonAsync<CheckTime>("CheckTimes/new", checkTime);
                return Data(checkTime);

            }
            catch (Exception e)
            {
                return Error(new CheckTime(), "Failed to Create CheckTime", e.Message);
            }
        }
        /// <summary>
        /// Deletes checktime
        /// </summary>
        /// <param name="id">Id of checktime</param>
        /// <returns><see cref="Boolean"/></returns>
        public async Task<APIWrapper<bool>> DeleteChecktime(long id)
        {
            try
            {
                await client.DeleteAsync($"CheckTimes/{id}");
                return Data(true);

            }
            catch (Exception e)
            {
                return Error(false, "Failed to delete CheckTime", e.Message);
            }
        }
        /// <summary>
        /// Gets a list of all checktimes
        /// </summary>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<List<CheckTime>>> GetCheckTimes()
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<CheckTime>>("CheckTimes"));

            }
            catch (Exception e)
            {
                return Error(new List<CheckTime>(), "Failed to get CheckTimes", e.Message);
            }
        }

        /// <summary>
        /// Gets Checktimes from card and date
        /// </summary>
        /// <param name="cardId">Card you want to use</param>
        /// <param name="date">The specific date you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<List<CheckTime>>> GetCheckTimes(string cardId, DateTime date)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<CheckTime>>($"CheckTimes/{cardId}/{date.ToString("yyyy-MM-dd")}"));

            }
            catch (Exception e)
            {
                return Error(new List<CheckTime>(), "Failed to get CheckTimes from card and date", e.Message);
            }
        }
        /// <summary>
        /// Gets checktimes from card and day
        /// </summary>
        /// <param name="cardId">Card you want to use</param>
        /// <param name="day">The day you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<List<CheckTime>>> GetCheckTimes(string cardId, DayOfWeek day)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<CheckTime>>($"CheckTimes/{cardId}/{day}"));

            }
            catch (Exception e)
            {
                return Error(new List<CheckTime>(), "Failed to get CheckTimes from card and day", e.Message);
            }
        }
        /// <summary>
        /// Gets latest checktime from cardid
        /// </summary>
        /// <param name="cardId">Card you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<CheckTime>> GetCheckTime(string cardId)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<CheckTime>($"CheckTimes/{cardId}"));

            }
            catch (Exception e)
            {
                return Error(new CheckTime(), "Failed to get CheckTime from cardid", e.Message);
            }
        }
        /// <summary>
        /// Gets Checktime from Id
        /// </summary>
        /// <param name="Id">Checktime Id</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<CheckTime>> GetCheckTime(long Id)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<CheckTime>($"CheckTimes/id/{Id}"));

            }
            catch (Exception e)
            {
                return Error(new CheckTime(), "Failed to get CheckTime from id", e.Message);
            }
        }
        /// <summary>
        /// Gets todays checktimes from card
        /// </summary>
        /// <param name="cardId">Card you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<List<CheckTime>>> Today(string cardId)
        {
            try
            {
                return Data(await client.GetFromJsonAsync<List<CheckTime>>($"CheckTimes/{cardId}/today"));

            }
            catch (Exception e)
            {
                return Error(new List<CheckTime>(), "Failed to get CheckTimes from today", e.Message);
            }
        }
        /// <summary>
        /// Update checktime
        /// </summary>
        /// <param name="id">Checktime Id</param>
        /// <param name="checkTime">Checktime you want to update</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public async Task<APIWrapper<CheckTime>> UpdateChecktime(long id, CheckTime checkTime)
        {
            try
            {
                await client.PutAsJsonAsync<CheckTime>($"CheckTimes/{id}", checkTime);
                return Data(checkTime);

            }
            catch (Exception e)
            {
                return Error(new CheckTime(), "Failed to update CheckTime", e.Message);
            }
        }

        public async Task<APIWrapper<bool>> ChangeCard(Card newCard, Card oldCard)
        {
            try
            {
                await client.PostAsJsonAsync<Card>($"CheckTimes/changecard/{newCard}", oldCard);
                return Data(true);

            }
            catch (Exception e)
            {
                return Error(false, "Failed to change card", e.Message);
            }
        }
    }
}
