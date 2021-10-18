using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    public interface IServices
    {
        /// <summary>
        /// Holds the actions for checktimes
        /// </summary>
        public ICheckTimes CheckTimes { get; }

        /// <summary>
        /// Holds the actions for Rooms
        /// </summary>
        public IRooms Rooms { get; }

        /// <summary>
        /// Holds the actions for Scanners
        /// </summary>
        public IScanners Scanners { get; }

        /// <summary>
        /// Holds the actions for Users
        /// </summary>
        public IUsers Users { get; }

        /// <summary>
        /// Holds the actions for YearSummeries
        /// </summary>
        public IYearSummeries YearSummeries { get; }
    }
}
