using CheckIn.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    /// <summary>
    /// Year summery
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class YearSummeryController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public YearSummeryController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: YearSummery/aa aa aa aa/2020.
        /// Gets all times from given card in given year and organizes them by week, day etc...
        /// </summary>
        /// <param name="cardID"> Card id to select </param>
        /// <param name="year"> Year to select </param>
        /// <returns></returns>
        [HttpGet("{cardID}/{year}")]
        public async Task<List<YearSummery>> GetScanners(string cardID, int year)
        {
            var checks = await _context.CheckTimes.Where(a => a.CardId == cardID).Where(a => a.Time.Year == year).OrderBy(a => a.Time).ToListAsync();
            List<YearSummery> output = new List<YearSummery>();


            DateTime beginDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year + 1, 01, 01);
            int week = 1;

            var currentweeksum = new YearSummery
            {
                Week = week
            };

            while (beginDate < endDate)
            {
                switch (beginDate.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        output.Add(currentweeksum);
                        week++;
                        currentweeksum = new YearSummery
                        {
                            Week = week
                        };
                        break;

                    case DayOfWeek.Saturday:
                        break;

                    default:
                        foreach (var item in checks)
                        {
                            if (item.Time.Date == beginDate.Date)
                            {
                                currentweeksum.Times[beginDate.DayOfWeek.ToString()].Add(item);
                            }
                        }
                        break;
                }
                beginDate = beginDate.AddDays(1);
            }

            output.Reverse();
            return output;
        }

    }
}
