using CheckIn.Frontend.Services;
using CheckIn.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using CheckIn.Frontend.Helper;
using System.Timers;
using CheckIn.Shared.Helpers;

namespace CheckIn.Frontend.Pages
{
    public partial class Index : IDisposable
    {
        [Inject]
        public IServices Services { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        private Timer aTimer;
        private DateTimeOffset LastTime;
        private DateTimeOffset FirstTime;
        private TimeSpan BreakTime = new TimeSpan();
        public string TimeDiff { get; set; }
        public string TotalTime { get; set; }

        [Parameter]
        public string Card { get; set; }

        public void Dispose() => aTimer?.Dispose();

        User modelUser;
        string studentCard;

        bool IsOtherUser = false;
        User User = new User();

        /// <summary>
        /// Table data cells
        /// </summary>
        class TimePoint
        {
            public string Time { get; set; }
            public string TimeDiff { get; set; }

            /// <summary>
            /// Will say if the cell is a checkin time
            /// </summary>
            public bool IsCheckedIn { get; set; }

            public TimePoint()
            {
                Time = "";
                TimeDiff = "";
                IsCheckedIn = false;
            }
        }

        // List of all times
        List<Dictionary<DayOfWeek, TimePoint>> TimePoints = new List<Dictionary<DayOfWeek, TimePoint>>();

        /// <summary>
        /// Adds an extra row to the tble
        /// </summary>
        /// <param name="amount">The amount of rows you wish to create</param>
        void AddRows(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var times = new Dictionary<DayOfWeek, TimePoint>();

                // Add each day to the row
                for (int n = 0; n <= 6; n++)
                    times.Add((DayOfWeek)n, new TimePoint());

                TimePoints.Add(times);
            }
        }

        /// <summary>
        /// Inserts the data into the table
        /// </summary>
        /// <param name="_checkTimes"></param>
        /// <returns></returns>
        Task CreateOverviewOfTheWeek(List<CheckTime> _checkTimes)
        {
            // Returns if no checktimes
            if (_checkTimes.Count == 0)
                return Task.CompletedTask;

            // Set every time to the users timezone
            foreach (var item in _checkTimes)
                item.Time = item.Time.ToLocalTime();

            // Adds row if needed
            if (TimePoints.Count < _checkTimes.Count)
                AddRows(_checkTimes.Count - TimePoints.Count);

            int row = 0;
            var day = _checkTimes.First().Time.DayOfWeek;

            // Add times if there are more than one
            if (_checkTimes.Count > 1)
            {

                for (row = 0; row < _checkTimes.Count - 1; row++)
                {
                    TimePoints[row][day] = new TimePoint
                    {
                        // Show a timepoint to another time point
                        Time = $"{_checkTimes[row].Time.ToString().Substring(11, 5)} til {_checkTimes[row + 1].Time.ToString().Substring(11, 5)}",

                        // Sets the time diffrence between the points
                        TimeDiff = (_checkTimes[row + 1].Time - _checkTimes[row].Time).ToReadableTime(),

                        // Will say if its a checkin time or not
                        IsCheckedIn = row % 2 == 0
                    };
                }
            }

            // Check if the table colum is today
            if (DateTime.Now.DayOfWeek == day)
            {
                // If the time is checked in then add a timer else display total active checkin time
                if (_checkTimes.Count % 2 != 0)
                {
                    TimePoints[row][day] = new TimePoint
                    {
                        Time = $"{_checkTimes.Last().Time.ToString("HH:mm")} til nu",
                        TimeDiff = "",
                        IsCheckedIn = true
                    };
                }
                else
                {
                    TimePoints[row][day] = new TimePoint
                    {
                        Time = $"{_checkTimes.Last().Time.ToString("HH:mm")} til nu",
                        TimeDiff = "",
                        IsCheckedIn = false
                    };
                }

                // If there is multiple times and you are checked in
                if (_checkTimes.Count > 1)
                {
                    // Adds a new row if needed
                    if (TimePoints.Count - 1 < ++row)
                        AddRows(1);

                    // If the time is checked in then add a timer else display total active checkin time
                    if (_checkTimes.Count % 2 != 0)
                    {
                        TimePoints[row][day] = new TimePoint
                        {
                            Time = "Total Tid:",
                            TimeDiff = "total",
                            IsCheckedIn = true
                        };
                    }
                    else
                    {
                        TimePoints[row][day] = new TimePoint
                        {
                            Time = "Total Tid:",
                            TimeDiff = GetActiveCheckInTime(_checkTimes),
                            IsCheckedIn = false
                        };
                    }
                }

                // Start a timer
                FirstTime = _checkTimes.First().Time;
                LastTime = _checkTimes.Last().Time;
                BreakTime = GetBreakTimes(_checkTimes);
                aTimer.Start();
            }
            else
            {
                // Check if the user is checked out or not
                if (_checkTimes.Count % 2 == 0)
                {
                    TimePoints[row][day] = new TimePoint
                    {
                        Time = $"Checket ud: {_checkTimes.Last().Time.ToString("HH:mm")}",
                        TimeDiff = GetActiveCheckInTime(_checkTimes)
                    };
                }
                else
                {
                    TimePoints[row][day] = new TimePoint
                    {
                        Time = $"{_checkTimes.Last().Time.ToString("HH:mm")}",
                        TimeDiff = "Glemte at check ud"
                    };
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the actual time you have been checked in
        /// </summary>
        /// <param name="checkTimes"></param>
        /// <returns>Formatted string with the actual time</returns>
        public string GetActiveCheckInTime(List<CheckTime> checkTimes)
        {
            TimeSpan totaleTime = new TimeSpan();
            for (int row = 0; row < checkTimes.Count - 1; row++)
            {
                if (row % 2 == 0)
                    totaleTime += checkTimes[row + 1].Time - checkTimes[row].Time;
                else
                    totaleTime -= checkTimes[row + 1].Time - checkTimes[row].Time;
            }

            return totaleTime.ToReadableTime();
        }

        /// <summary>
        /// The Amount of time a user has been on break
        /// </summary>
        /// <param name="checkTimes"></param>
        /// <returns></returns>
        public TimeSpan GetBreakTimes(List<CheckTime> checkTimes)
        {
            TimeSpan totaleTime = new TimeSpan();
            for (int row = 0; row < checkTimes.Count - 1; row++)
            {
                if (row % 2 != 0)
                    totaleTime -= checkTimes[row + 1].Time - checkTimes[row].Time;
            }

            return totaleTime;
        }

        protected override async Task OnInitializedAsync()
        {
            // Set timer settings
            aTimer = new Timer(1000);
            aTimer.Elapsed += TimerTicked; ;
            aTimer.AutoReset = true;

            // Get the user of the card we are intrested in
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
            modelUser = user.ToUser();
            studentCard = modelUser.CardId;

            if (Card != null && Card != "")
            {
                studentCard = Card;
                var result = await Services.Users.GetUsersFromCards(Card);
                if (result.HasSucceded && result.Result.Count != 0)
                {
                    User = result.Result.FirstOrDefault();
                    IsOtherUser = true;
                }
            }

            // Seeds the table with the users time
            var today = DateTime.Now;
            for (int i = 0; i < (int)today.DayOfWeek; i++)
            {
                // Checks if the current day is Saturday or Sunday. If so, don't fetch data from those days
                if ((int)today.DayOfWeek < 6)
                {
                    var day = today.AddDays(-i);
                    var result = await Services.CheckTimes.GetCheckTimes(studentCard, day.Date);

                    if (result.Result.Count > 0)
                        await CreateOverviewOfTheWeek(result.Result);
                }
            }
        }

        /// <summary>
        /// Triggers once a secound to count up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTicked(object sender, ElapsedEventArgs e)
        {
            var currentTime = DateTime.Now;
            TimeDiff = (currentTime - LastTime).ToReadableTime();
            TotalTime = ((currentTime - FirstTime) + BreakTime).ToReadableTime();

            StateHasChanged();
        }
    }
}