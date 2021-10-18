using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    public interface IRooms
    {
        /// <summary>
        /// Get all rooms
        /// </summary>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<List<Room>>> GetRooms();

        /// <summary>
        /// Gets room from id
        /// </summary>
        /// <param name="id">Room Id</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<Room>> GetRoom(int id);

        /// <summary>
        /// Get room by card
        /// </summary>
        /// <param name="cardID">Card you want room from</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<Room>> GetRoomsByCard(string cardID);

        /// <summary>
        /// Get rooms by departments
        /// </summary>
        /// <param name="dep"></param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<List<Room>>> GetRoomsByDepartments(string[] dep);

        /// <summary>
        /// Create new room
        /// </summary>
        /// <param name="room">Room you want to create</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<Room>> CreateRoom(Room room);

        /// <summary>
        /// Move card to room
        /// </summary>
        /// <param name="roomId">Room id you want card in</param>
        /// <param name="card">Card you want to move to room</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<Room>> MoveCardToRoom(int roomId, Card card);

        /// <summary>
        /// Removes card from room
        /// </summary>
        /// <param name="roomId">Room id you want card removed from</param>
        /// <param name="cardId">Card you want to remove</param>
        /// <returns><see cref="Boolean"/></returns>
        public Task<APIWrapper<bool>> RemoveCardFromRoom(int roomId, string cardId);

        /// <summary>
        /// Update room name or scanner
        /// </summary>
        /// <param name="id">Room id</param>
        /// <param name="room">Room you want to update</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="Room"/></returns>
        public Task<APIWrapper<Room>> UpadteRoom(int id, Room room);

        /// <summary>
        /// Deletes room
        /// </summary>
        /// <param name="id">Room id</param>
        /// <returns><see cref="Boolean"/></returns>
        public Task<APIWrapper<bool>> DeleteRoom(int id);
    }
}
