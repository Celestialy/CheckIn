using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    public interface IUsers
    {
        /// <summary>
        /// Gets users from an array of cards
        /// </summary>
        /// <param name="cards"></param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="User"/></returns>
        public Task<APIWrapper<List<User>>> GetUsersFromCards(params string[] cards);
        /// <summary>
        /// Gets users from an array of cards
        /// </summary>
        /// <param name="cards">List of cards you want to get users from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="User"/></returns>
        public Task<APIWrapper<List<User>>> GetUsersFromCards(List<Card> cards);

        /// <summary>
        /// Get users
        /// </summary>
        /// <param name="pagination">Pagination configuration</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="PagedList{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<PagedList<User>>> GetUsers(Pagination pagination);

        /// <summary>
        /// Add card to user api
        /// </summary>
        /// <param name="card"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<APIWrapper<bool>> AddCardToUser(string card, string userId);
    }
}
