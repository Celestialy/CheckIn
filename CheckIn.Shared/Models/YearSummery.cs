using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Shared.Models
{
    /// <summary>
    /// Used to genarate a year summery
    /// </summary>
    public class YearSummery
    {
        /// <summary>
        /// Week number
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// Times for each day in this week
        /// </summary>
        public Dictionary<string, List<CheckTime>> Times { get; set; }

        public YearSummery()
        {
            DayOfWeek[] workWeeks =
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };

            Times = new Dictionary<string, List<CheckTime>>();
            foreach (var week in workWeeks)
            {
                Times.Add(week.ToString(), new List<CheckTime>());
            }
        }
    }
}
