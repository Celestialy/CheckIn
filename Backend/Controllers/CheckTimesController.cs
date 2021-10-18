using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.SignalR;
using Backend.Hubs;
using CheckIn.Shared.IHubs;
using CheckIn.Shared.Models;
using CheckIn.Shared.Models.Types;
using Microsoft.Extensions.Logging;
using Backend.Helpers;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CheckTimesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IHubContext<CheckInHub, ICheckInHub> _hubContext;

        public CheckTimesController(DatabaseContext context, IHubContext<CheckInHub, ICheckInHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        /// <summary>
        /// GET: Cards/aa aa aa aa.
        /// Gets latest time from given card
        /// </summary>
        /// <param name="cardID"> CardId to get </param>
        /// <returns></returns>
        [HttpGet("{cardID}")]
        public async Task<ActionResult<CheckTime>> GetCheckTime(string cardID)
        {
            return await _context.CheckTimes.Include(a => a.Scanner).Where(a => a.CardId == cardID).OrderByDescending(a => a.ID).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GET: CheckTimes.
        /// Get every time from every card
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<CheckTime>>> GetCheckTimes()
        {
            return await _context.CheckTimes.Include(a => a.Scanner).OrderByDescending(a => a.ID).ToListAsync();
        }

        /// <summary>
        /// GET: CheckTimes/id/137.
        /// Get time from given id
        /// </summary>
        /// <param name="id">Id to find</param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<CheckTime>> GetCheckTime(long id)
        {
            return await _context.CheckTimes.Include(a => a.Scanner).Where(a => a.ID == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GET: CheckTimes/aa aa aa aa/2020-09-02.
        /// Gets Times on a spesific day from given card
        /// </summary>
        /// <param name="cardID"> CardId to get </param>
        /// <param name="date"> Date to select from </param>
        /// <returns></returns>
        [HttpGet("{cardID}/{date}")]
        public async Task<ActionResult<IEnumerable<CheckTime>>> GetCheckTimes(string cardID, DateTime date)
        {
            return await _context.CheckTimes
                .Where(a => a.CardId == cardID)
                .Where(a => a.Time.Date == date.Date)
                .OrderBy(a => a.Time)
                .ToListAsync();
        }

        /// <summary>
        /// GET: CheckTimes/aa aa aa aa/today.
        /// Get times from given card on today
        /// </summary>
        /// <param name="cardID"> CardId to select </param>
        /// <returns></returns>
        [HttpGet("{cardID}/today")]
        public async Task<ActionResult<IEnumerable<CheckTime>>> GetCheckTimes(string cardID)
        {
            return await _context.CheckTimes
                .Where(a => a.CardId == cardID)
                .Where(a => a.Time.Date == DateTime.UtcNow.Date)
                .OrderBy(a => a.Time)
                .ToListAsync();
        }

        /// <summary>
        /// GET: CheckTimes/day/aa aa aa aa/monday.
        /// Gets times from this monday friday or whatever
        /// </summary>
        /// <param name="cardID"> CardId to select </param>
        /// <param name="day"> day of week to select </param>
        /// <returns></returns>
        [HttpGet("day/{cardID}/{day}")]
        public async Task<ActionResult<IEnumerable<CheckTime>>> GetCheckTimes(string cardID, DayOfWeek day)
        {
            // Calculate datetime from day
            var now = DateTime.UtcNow;
            var diff = (7 + (now.DayOfWeek - day)) % 7;
            var wantedDay = now.AddDays(-1 * diff).Date;
            
            // Fix for wantedDay becoming a day in last week when a day hasent happened yet, like friday if it is monday
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = wantedDay.Date.AddDays(-1 * (int)cal.GetDayOfWeek(wantedDay));
            var d2 = now.Date.AddDays(-1 * (int)cal.GetDayOfWeek(now));
            if (d1 != d2)
            {
                string[] arr = new string[0];
                return Ok(arr);
            }

            return await _context.CheckTimes
                .Where(a => a.CardId == cardID)
                .Where(a => a.Time.Date == wantedDay.Date)
                .OrderBy(a => a.Time)
                .ToListAsync();
        }

        /// <summary>
        /// PUT: CheckTimes/5.
        /// Change the time on a checktime
        /// </summary>
        /// <param name="id"> Id of checktime to update</param>
        /// <param name="checkTime"> updated checktime </param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<IActionResult> PutCheckTime(long id, CheckTime checkTime)
        {
            var user = User.ToUser();
            if (id != checkTime.ID)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} just tried to update diffrent checktime one: {id} checktime to: {checkTime.ID}", Type = LogType.CheckTime });
                await _context.SaveChangesAsync();
                return BadRequest();
            }

            var check = await _context.CheckTimes.Where(a => a.ID == id).FirstOrDefaultAsync();

            if (check == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Warning, Msg = $"{user.Name} just tried to update checktime: {id} that doesnt exist", Type = LogType.CheckTime });
                await _context.SaveChangesAsync();

                return NotFound();
            }

            _context.Loggings.Add(new Logging { Action = LogAction.Update, Level = LogLevel.Information, Msg = $"{user.Name} just updated checktime: {id} from time {check.Time} to {checkTime.Time}", Type = LogType.CheckTime });
            check.Time = checkTime.Time;
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Groups(new List<string>{ "Instructor", "Administrator", check.CardId }).UpdateCheckTime(check);

            return NoContent();
        }

        /// <summary>
        /// DELETE: Checktime/5.
        /// Deletes a checktime
        /// </summary>
        /// <param name="id"> Id of checktime to delete </param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<IActionResult> DeleteCheckTime(long id)
        {
            var time = await _context.CheckTimes.FindAsync(id);
            var user = User.ToUser();

            if (time == null)
            {
                _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Warning, Msg = $"{user.Name} just tried to delete a checktime that doesnt exist ", Type = LogType.CheckTime });
                await _context.SaveChangesAsync();
                return NotFound();
            }

            _context.Loggings.Add(new Logging { Action = LogAction.Delete, Level = LogLevel.Information, Msg = $"{user.Name} just deleted a checktime for card: {time.CardId} ", Type = LogType.CheckTime });
            _context.CheckTimes.Remove(time);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// POST: CheckTimes/new
        /// Check in/out a cardID
        /// </summary>
        /// <param name="post"> Checktime to add </param>
        /// <returns></returns>
        [HttpPost("new")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<ActionResult<CheckTime>> PostCheckTime(CheckTime post)
        {
            // Check if scanner is allowed 
            var scanner = _context.Scanners.Find(post.Scanner.MacAddress);
            if (scanner != null)
            {
                var user = User.ToUser();

                // check if card is excist
                var cardExcist = (await _context.Cards.AnyAsync(x => x._card == post.CardId));
                if (!cardExcist)
                {
                    _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Warning, Msg = $"{user.Name} just tried to create a checktime with a card: {post.CardId} that doesnt exist ", Type = LogType.CheckTime });
                    await _context.SaveChangesAsync();
                    return NotFound("Card not found");
                }

                post.Scanner = scanner;
                _context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Information, Msg = $"{user.Name} just tried to created a new checktime for card: {post.CardId}", Type = LogType.CheckTime });
                _context.CheckTimes.Add(post);

                await _context.SaveChangesAsync();
                await _hubContext.Clients.Groups(new List<string> { "Instructor", "Administrator", post.CardId}).UpdateCheckTime(post);

                return Ok(post);
            }
            else
                return NotFound("Scanner not found");
        }

        /// <summary>
        /// POST: CheckTimes.
        /// Check in/out a cardID 
        /// </summary>
        /// <param name="post"> scanners post model </param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CheckTime>> PostCheckTime(ScannerPost post)
        {
            // Check if scanner is allowed 
            var scanner = _context.Scanners.Find(post.macAddress);
            if (scanner != null)
            {
                // check if card is excist
                var cardExcist = (await _context.Cards.AnyAsync(x => x._card == post.cardID));

                // Check if they have already checked in/out in the last 5 minutes
                var latestTime = await _context.CheckTimes.Where(a => a.CardId == post.cardID).OrderByDescending(a => a.Time).FirstOrDefaultAsync();
                DateTimeOffset now = DateTime.UtcNow;
                if (latestTime != null)
                {
                    if (latestTime.Time <= now && latestTime.Time > now.AddMinutes(-1))
                    {
                        //_context.Loggings.Add(new Logging { Action = LogAction.Read, Level = LogLevel.Warning, Msg = $"{post.cardID} just got scanned again within 5 minutes", Type = LogType.CheckTime });
                        //await _context.SaveChangesAsync();
                        return Conflict("TOO FAST");                        
                    }
                }

                // Add check time to database
                var checkTime = new CheckTime { CardId = post.cardID, Time = DateTime.UtcNow, Scanner = scanner };

                _context.CheckTimes.Add(checkTime);
                //_context.Loggings.Add(new Logging { Action = LogAction.Create, Level = LogLevel.Information, Msg = $"{checkTime.ID} just got created when {post.cardID} got scanned", Type = LogType.CheckTime });
                
                await _context.SaveChangesAsync();

                //sends signalr response with card scanned
                CardScanned cardScanned = new CardScanned();
                cardScanned.scanner = scanner;
                cardScanned.card = post.cardID;
                cardScanned.isNewCard = !cardExcist;

                //gets todays times and chek if the person is checking in or out
                var todaysTimes =  await _context.CheckTimes
                .Where(a => a.CardId == post.cardID)
                .Where(a => a.Time.Date == DateTime.UtcNow.Date)
                .OrderBy(a => a.Time)
                .ToListAsync();

                if (todaysTimes.Count != 0)
                {
                    cardScanned.CheckinTime = todaysTimes.First().Time;
                    cardScanned.ChecktimeId = todaysTimes.Last().ID;
                }
                if (todaysTimes.Count%2 != 0)
                    cardScanned.IsCheckingIn = true;

                try
                {
                    //ask user api after the user for the current card
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://skpauth.azurewebsites.net/");
                    client.DefaultRequestHeaders.Add("Api-key", "393a5414-4fea-4dc0-aa28-38f35e23f694");

                    var users = await client.GetFromJsonAsync<List<User>>($"ApiKey/Card/GetUsersFromCards?cards={cardScanned.card}&Fields=Name");
                    if (users.Count != 0)
                        cardScanned.Username = users.FirstOrDefault().Name;
                    else
                        cardScanned.Username = "Bruger er ikke tildelt kort.";
                }
                catch (Exception)
                {
                    cardScanned.Username = "Bruger er ikke tildelt kort.";
                }

                await _hubContext.Clients.Groups(new List<string> {"Public" }).CardScanned(cardScanned);

                // If unknown card, send notfound so scanner beebs red (error)
                if (cardExcist)
                    return Ok(checkTime);
                else
                    return NotFound(checkTime);
            }
            else
                return NotFound("Scanner dosent exit");

        }

        [HttpPost("changecard/{newCard}")]
        [Authorize(Roles = "Instructor,Administrator")]
        public async Task<IActionResult> ChangeCard(string newCard, Card oldCard)
        {
            if (_context.CheckTimes.Any(x => x.CardId == oldCard))
            {
                await _context.CheckTimes.Where(x => x.CardId == oldCard).ForEachAsync(x =>
                {
                     x.CardId = newCard;
                });
            }
            
            if (_context.Rooms.Any(x => x.Cards.Any(y => y._card == oldCard._card)))
            {
                var room = _context.Rooms.Include(c => c.Cards).Where(x => x.Cards.Any(y => y._card == oldCard)).First();


                room.Cards.RemoveAll(x => x._card == oldCard._card);
                room.Cards.Add(newCard);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
