using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    /// <inheritdoc/>
    public class CollectionOfServices : IServices
    {


        public ICheckTimes CheckTimes { get; private set; }
        public IRooms Rooms { get; private set; }
        public IScanners Scanners { get; private set; }
        public IUsers Users { get; private set; }
        public IYearSummeries YearSummeries { get; private set; }
        public CollectionOfServices( ICheckTimes checkTimes, IRooms rooms, IScanners scanners, IUsers users, IYearSummeries yearSummeries)
        {
            CheckTimes = checkTimes;
            Rooms = rooms;
            Scanners = scanners;
            Users = users;
            YearSummeries = yearSummeries;
        }
    }
}
