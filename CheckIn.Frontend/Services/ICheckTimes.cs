using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    public interface ICheckTimes
    {
        /// <summary>
        /// Gets a list of all checktimes
        /// </summary>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<List<CheckTime>>> GetCheckTimes();

        /// <summary>
        /// Gets Checktimes from card and date
        /// </summary>
        /// <param name="cardId">Card you want to use</param>
        /// <param name="date">The specific date you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<List<CheckTime>>> GetCheckTimes(string cardId, DateTime date);

        /// <summary>
        /// Gets checktimes from card and day
        /// </summary>
        /// <param name="cardId">Card you want to use</param>
        /// <param name="day">The day you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<List<CheckTime>>> GetCheckTimes(string cardId, DayOfWeek day);

        /// <summary>
        /// Gets latest checktime from cardid
        /// </summary>
        /// <param name="cardId">Card you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<CheckTime>> GetCheckTime(string cardId);

        /// <summary>
        /// Gets Checktime from Id
        /// </summary>
        /// <param name="Id">Checktime Id</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<CheckTime>> GetCheckTime(long Id);

        /// <summary>
        /// Gets todays checktimes from card
        /// </summary>
        /// <param name="cardId">Card you want times from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="CheckTime"/></returns> 
        public Task<APIWrapper<List<CheckTime>>> Today(string cardId);

        /// <summary>
        /// Update the time
        /// </summary>
        /// <param name="id">Checktime Id</param>
        /// <param name="checkTime">Checktime you want to update the time for</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<CheckTime>> UpdateChecktime(long id, CheckTime checkTime);

        /// <summary>
        /// Deletes a checktime
        /// </summary>
        /// <param name="id">CheckTime to delete</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<bool>> DeleteChecktime(long id);

        /// <summary>
        /// Creates a new checktime
        /// </summary>
        /// <param name="checkTime">Checktime to create</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<CheckTime>> CreateChecktime(CheckTime checkTime);

        /// <summary>
        /// Changes the card of a user
        /// </summary>
        /// <param name="newCard">Old cardID</param>
        /// <param name="oldCard">New cardID</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="CheckTime"/></returns>
        public Task<APIWrapper<bool>> ChangeCard(Card newCard, Card oldCard);
    }
}
