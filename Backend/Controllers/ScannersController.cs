using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend;
using Microsoft.AspNetCore.Authorization;
using CheckIn.Shared.Models;
using Backend.Helpers;
using CheckIn.Shared.Models.Types;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    /// <summary>
    /// Manages scanners
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ScannersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ScannersController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Scanners.
        /// Get all scanners
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<IEnumerable<Scanner>>> GetScanners()
        {
            return await _context.Scanners.ToListAsync();
        }

        /// <summary>
        /// GET: Scanners/5.
        /// Get scanner from given Id
        /// </summary>
        /// <param name="id"> Id of scanner to get </param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<Scanner>> GetScanner(string id)
        {
            var scanner = await _context.Scanners.FindAsync(id);

            if (scanner == null)
            {
                return NotFound();
            }

            return scanner;
        }

        /// <summary>
        /// POST: Scanners.
        /// Create new scanner
        /// </summary>
        /// <param name="scanner"> Scanner to create </param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<Scanner>> PostScanner(Scanner scanner)
        {
            scanner.Added = DateTime.Now;
            var user = User.ToUser();
            try
            {
                _context.Scanners.Add(scanner);
                _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Information, Msg = $"{user.Name} created scanner: {scanner.MacAddress}", Type = LogType.Scanner });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ScannerExists(scanner.MacAddress))
                {
                    _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Warning, Msg = $"{user.Name} tried to create a scanner: {scanner.MacAddress} that allready excist", Type = LogType.Scanner });
                    await _context.SaveChangesAsync();
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetScanner", new { id = scanner.MacAddress }, scanner);
        }

        /// <summary>
        /// DELETE: Scanners/5.
        /// Delete scanner 
        /// </summary>
        /// <param name="id"> Id of scanner to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<Scanner>> DeleteScanner(string id)
        {
            var scanner = await _context.Scanners.FindAsync(id);
            var user = User.ToUser();
            if (scanner == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Warning, Msg = $"{user.Name} tried to remove a scanner: {id} that doesn't excist", Type = LogType.Scanner });
                await _context.SaveChangesAsync();
                return NotFound();
            }

            var rooms = await _context.Rooms.Where(a => a.Scanner == scanner).ToListAsync();
            var checkTimes = await _context.CheckTimes.Where(a => a.Scanner == scanner).ToListAsync();

            if (rooms.Count != 0)
            {
                foreach (var room in rooms)
                {
                    room.Scanner = null;
                }
            }

            if (checkTimes.Count != 0)
            {
                foreach (var time in checkTimes)
                {
                    time.Scanner = null;
                }
            }
            _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Information, Msg = $"{user.Name} deleted scanner: {scanner.MacAddress}", Type = LogType.Scanner });
            _context.Scanners.Remove(scanner);
            await _context.SaveChangesAsync();

            return scanner;
        }

        /// <summary>
        /// POST: Scanners/5.
        /// Edit scanner 
        /// </summary>
        /// <param name="id"> Id of scanner to edit</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<Scanner>> EditScanner(string id, Scanner newScanner)
        {
            var user = User.ToUser();

            if (id != newScanner.MacAddress)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} tried to update scanner's name with Mac Address: '{newScanner.MacAddress}' to '{newScanner.Name}'", Type = LogType.Scanner });
                await _context.SaveChangesAsync();
                return BadRequest();
            }

            var _scanner = await _context.Scanners.Where(x => x.MacAddress == id).FirstOrDefaultAsync();

            if (_scanner == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} tried to update scanner: {id} that doesnt exist", Type = LogType.Scanner });
                await _context.SaveChangesAsync();
                return NotFound();
            }

            _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Information, Msg = $"Updated scanner name with Mac Address: {newScanner.MacAddress} to {newScanner.Name}", Type = LogType.Scanner });
            _scanner.Name = newScanner.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ScannerExists(string id)
        {
            return _context.Scanners.Any(e => e.MacAddress == id);
        }
    }
}
