using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CheckIn.Shared.Models;
using CheckIn.Shared.Models.Types;
using Microsoft.Extensions.Logging;
using Backend.Helpers;

namespace Backend.Controllers
{
    /// <summary>
    /// Manages rooms
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public RoomsController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Rooms.
        /// Get all rooms with the amount of students in each room
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return Ok(await _context.Rooms.Select(a => new { a.ID, a.RoomName, a.Department, a.Cards,  StudentsAmount = a.Cards.Count , a.Added }).ToListAsync());
        }

        /// <summary>
        /// GET: Rooms/5.
        /// Gets a room based on room Id given
        /// </summary>
        /// <param name="id"> Id of room to get </param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRooms(int id)
        {
            var room = await _context.Rooms.Include(a => a.Cards).Include(a => a.Scanner).Where(a => a.ID == id).FirstOrDefaultAsync();

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        /// <summary>
        /// GET: Rooms/card/aa aa aa aa.
        /// Find room for given cardId
        /// </summary>
        /// <param name="cardID"> CardId to find a room for </param>
        /// <returns></returns>
        [HttpGet("card/{cardID}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsByCard(string cardID)
        {

            var room = await _context.Rooms.Where(a => a.Cards.Contains(cardID)).FirstOrDefaultAsync();
            
            
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        /// <summary>
        /// GET: Rooms/department?dep=Data.
        /// Gets a room based on department name given
        /// </summary>
        /// <param name="dep"> Departments to find rooms for </param>
        /// <returns></returns>
        [HttpGet("department")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms([FromQuery]string[] dep)
        {
            var rooms = await _context.Rooms.Include(a => a.Scanner).Include(a => a.Cards).Where(a => dep.Any(x => x == a.Department)).ToListAsync();

            return Ok(rooms);
        }

        /// <summary>
        /// POST: Rooms.
        /// Create new room
        /// </summary>
        /// <param name="room"> room to create </param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<Room>> PostRooms(Room room)
        {
            var dep = User.Claims.Where(a => a.Type == "department").Select(a => a.Value.Split(":")[1]).ToList();
            var user = User.ToUser();
            if (!dep.Contains(room.Department))
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Warning, Msg = $"{user.Name} just tried to create a room in a department they are not in", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return BadRequest("No access to " + room.Department);
            }

            var now = DateTime.Now;
            room.Added = now;

            var scanner = await _context.Scanners.FindAsync(room.Scanner.MacAddress);

            if (scanner == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Warning, Msg = $"{user.Name} just tried to create a room without a scanner", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return NotFound("Can't find scanner");
            }

            room.Scanner = scanner;

            _context.Rooms.Add(room);
            _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Information, Msg = $"{user.Name} just created room: {room.ID}", Type = LogType.Room });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRooms", new { id = room.ID }, room);
        }

        /// <summary>
        /// POST: Rooms/card/5.
        /// Move given card to given room
        /// </summary>
        /// <param name="roomid"> RoomId to move card to</param>
        /// <param name="card"> Card to move </param>
        /// <returns></returns>
        [HttpPost("card/{roomid}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<Room>> PostRooms(int roomid, Card card)
        {
            
            var room = await _context.Rooms.Include(a => a.Cards).Where(a => a.ID == roomid).FirstOrDefaultAsync();
            var currentCard = await _context.Cards.FindAsync(card._card);
            var user = User.ToUser();
            if (room == null || card == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} Forgot to include room or card", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return NotFound();
            }
            //if (dbcard == null)
            //{
            //    room.Cards.Add(card);
            //    _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Information, Msg = $"{user.Name} added Card: {card} to room: {room.ID} and created new card: {card}", Type = LogType.Room });
            //}
            else if (room.Cards.Any(x => x._card == card._card))
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} Card: {card} is allready in room: {room.ID}", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return Conflict("Card already added");
            }
            else
            {
                if (currentCard != null)
                {
                    currentCard.RoomId = roomid;
                }
                else
                {
                    room.Cards.Add(card);
                }
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Information, Msg = $"{user.Name} added Card: {card} to room: {room.ID}", Type = LogType.Room });
            }


            await _context.SaveChangesAsync();

            return Ok(room);
        }

        /// <summary>
        /// DELETE: Rooms/card/5/aa aa aa aa.
        /// Remove card from room
        /// </summary>
        /// <param name="roomid"> RoomId to remove from </param>
        /// <param name="cardID"> CardId to remove </param>
        /// <returns></returns>
        [HttpDelete("card/{roomid}/{cardID}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<Room>> RemoveCard(int roomid, string cardID)
        {
            var dbcard = await _context.Cards.FindAsync(cardID);
            var room = await _context.Rooms.Include(a => a.Cards).Where(a => a.ID == roomid).FirstOrDefaultAsync();
            var user = User.ToUser();
            if (dbcard == null || room == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Warning, Msg = $"{user.Name} Forgot to include room or card", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return NotFound();
            }
            room.Cards.Remove(dbcard);
            _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Information, Msg = $"{user.Name} removed Card: {dbcard} from room: {room.ID}", Type = LogType.Room });
            await _context.SaveChangesAsync();

            return Ok(room);
        }

        /// <summary>
        /// PUT: Rooms/5.
        /// edit room name and scanner
        /// </summary>
        /// <param name="id"> Id of room to edit </param>
        /// <param name="room"> updated room </param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            var scanner = await _context.Scanners.FindAsync(room.Scanner.MacAddress);
            var oldroom = await _context.Rooms.FindAsync(room.ID);
            var user = User.ToUser();
            if (id != room.ID || scanner == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} Forgot to include room or scanner", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return BadRequest();
            }

            if (oldroom == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} Room doesnt excist", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return NotFound();
            }
            oldroom.RoomName = room.RoomName;
            oldroom.Scanner = scanner;
            _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Information, Msg = $"{user.Name} updated room: {room.ID}, room name: {oldroom.RoomName}, scanner: {scanner.MacAddress} ", Type = LogType.Room });
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// DELETE: Rooms/5.
        /// Delete room
        /// </summary>
        /// <param name="id"> Room Id to delete </param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Room>> DeleteRooms(int id)
        {
            var rooms = await _context.Rooms.Include(a => a.Cards).Where(a => a.ID == id).FirstOrDefaultAsync();
            var user = User.ToUser();
            if (rooms == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Warning, Msg = $"{user.Name} Room doesnt excist", Type = LogType.Room });
                await _context.SaveChangesAsync();
                return NotFound();
            }

            if (rooms.Cards.Count > 0)
            {
                rooms.Cards = new List<Card>();
            }
            _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Information, Msg = $"{user.Name} removed room: {rooms.ID}, room name: {rooms.RoomName}", Type = LogType.Room });
            _context.Rooms.Remove(rooms);
            await _context.SaveChangesAsync();

            return rooms;
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.ID == id);
        }
    }
}
