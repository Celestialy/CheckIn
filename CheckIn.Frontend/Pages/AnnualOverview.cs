using CheckIn.Frontend.Helper;
using CheckIn.Frontend.Services;
using CheckIn.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Pages
{
    public partial class AnnualOverview
    {
        [Inject]
        public IServices Services { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Parameter]
        public string Card { get; set; }
        User modelUser;
        string studentCard = "";

        bool progressBarDisabled = true;
        bool atCurrentYear = true;
        bool hideEditTime = true;

        // Cheks if we see another users times
        bool IsOtherUser = false;

        // Checks if user dependency is loaded
        bool UserIsLoaded = false;

        /// <summary>
        /// checks if users are loaded and have permssions to edit time
        /// </summary>
        public bool CanManageTimes
        {
            get
            {
                if (UserIsLoaded)
                {
                    if (User.Roles.Any(x => x.Name == "Instructor") && modelUser.Roles.Any(x => x.Name == "Administrator"))
                        return true;
                    else if (!User.Roles.Any(x => x.Name == "Instructor") && (modelUser.Roles.Any(x => x.Name == "Administrator") || modelUser.Roles.Any(x => x.Name == "Instructor")))
                        return true;
                }

                return false;
            }
        }

        public bool CanCreateTime
        {
            get
            {
                return scannersList.Any(x => x.Name == selectedScanner);
            }
        }

        string selectedDate, selectedScanner;
        int selectedYear, selectedWeek;
        long selectedDateCheckInID, selectedDateCheckOutID;

        List<string> scannerNames = new();
        List<Scanner> scannersList = new();
        List<YearSummery> yearSummeries = new();

        User User = new();
        DateTimeOffset newCheckDateTime = DateTimeOffset.Now;

        public class OutPut
        {
            public int Week { get; set; }

            public long MondayInID { get; set; }
            public string MondayIn { get; set; }
            public long MondatOutID { get; set; }
            public string MondayOut { get; set; }
            public string MondayDate { get; set; }

            public long TuesdayInID { get; set; }
            public string TuesdayIn { get; set; }
            public long TuesdayOutID { get; set; }
            public string TuesdayOut { get; set; }
            public string TuesdayDate { get; set; }

            public long WednesdayInID { get; set; }
            public string WednesdayIn { get; set; }
            public long WednesdayOutID { get; set; }
            public string WednesdayOut { get; set; }
            public string WednesdayDate { get; set; }

            public long ThursdayInID { get; set; }
            public string ThursdayIn { get; set; }
            public long ThursdayOutID { get; set; }
            public string ThursdayOut { get; set; }
            public string ThursdayDate { get; set; }

            public long FridayInID { get; set; }
            public string FridayIn { get; set; }
            public long FridayOutID { get; set; }
            public string FridayOut { get; set; }
            public string FridayDate { get; set; }
        }

        List<OutPut> OutPutList;

        /// <summary>
        /// Gets the check in and check out times for the specified year
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        async Task GetChecktimes(string cardId, int year)
        {
            progressBarDisabled = false;
            OutPutList = new List<OutPut>();
            selectedYear = year;

            if (year >= DateTime.Now.Year)
                atCurrentYear = true;
            else
                atCurrentYear = false;

            var result = await Services.YearSummeries.GetYearSummeries(cardId, year);
            yearSummeries = result.Result;

            string[] date = new string[3];
            string checkInTime = "";
            string checkOutTime = "";
            long checkInID, checkOutID;

            OutPut currentWeek = new OutPut();
            List<CheckTime> checkTimes = new List<CheckTime>();

            foreach (var week in yearSummeries)
            {
                foreach (var times in week.Times)
                {
                    checkTimes = times.Value;
                    if (checkTimes.Count > 0)
                    {
                        checkInID = 0;
                        checkInTime = "00.00.0000 00.00 +00.00";
                        if (checkTimes.Count >= 1)
                        {
                            checkInID = checkTimes.First().ID;
                            checkInTime = checkTimes.First().Time.ToLocalTime().ToString();
                        }

                        checkOutID = 0;
                        checkOutTime = "00.00.0000 00.00 +00.00";
                        if (checkTimes.Count > 1)
                        {
                            checkOutID = checkTimes.Last().ID;
                            checkOutTime = checkTimes.Last().Time.ToLocalTime().ToString();
                        }

                        date[0] = checkInTime.Substring(11, 5);  // Check in time
                        date[1] = checkOutTime.Substring(11, 5); // Check out time
                        date[2] = checkInTime.Substring(0, 10);  // Date of day

                        date[0] = date[0].Replace('.', ':');
                        date[1] = date[1].Replace('.', ':');

                        // Gets the day of the week
                        DateTime dateTime = new DateTime(
                            Convert.ToInt32(date[2].Substring(6, 4)),
                            Convert.ToInt32(date[2].Substring(3, 2)),
                            Convert.ToInt32(date[2].Substring(0, 2)));

                        if (checkInID == 0)
                            date[0] = "";

                        if (checkOutID == 0)
                            date[1] = "";

                        // Fills in the table with check in and out times
                        switch ((int)dateTime.DayOfWeek)
                        {
                            case 1:
                                #region Monday
                                if (currentWeek.MondayIn == null)
                                {
                                    currentWeek.MondayIn = date[0];
                                    currentWeek.MondayInID = checkInID;
                                }

                                if (currentWeek.MondayOut == null)
                                {
                                    currentWeek.MondayOut = date[1];
                                    currentWeek.MondatOutID = checkOutID;
                                }

                                currentWeek.MondayDate = date[2];
                                #endregion
                                break;

                            case 2:
                                #region Tuesday
                                if (currentWeek.TuesdayIn == null)
                                {
                                    currentWeek.TuesdayIn = date[0];
                                    currentWeek.TuesdayInID = checkInID;
                                }

                                if (currentWeek.TuesdayOut == null)
                                {
                                    currentWeek.TuesdayOut = date[1];
                                    currentWeek.TuesdayOutID = checkOutID;
                                }

                                currentWeek.TuesdayDate = date[2];
                                #endregion
                                break;

                            case 3:
                                #region Wednesday
                                if (currentWeek.WednesdayIn == null)
                                {
                                    currentWeek.WednesdayIn = date[0];
                                    currentWeek.WednesdayInID = checkInID;
                                }

                                if (currentWeek.WednesdayOut == null)
                                {
                                    currentWeek.WednesdayOut = date[1];
                                    currentWeek.WednesdayOutID = checkOutID;
                                }

                                currentWeek.WednesdayDate = date[2];
                                #endregion
                                break;

                            case 4:
                                #region Thursday
                                if (currentWeek.ThursdayIn == null)
                                {
                                    currentWeek.ThursdayIn = date[0];
                                    currentWeek.ThursdayInID = checkInID;
                                }

                                if (currentWeek.ThursdayOut == null)
                                {
                                    currentWeek.ThursdayOut = date[1];
                                    currentWeek.ThursdayOutID = checkOutID;
                                }

                                currentWeek.ThursdayDate = date[2];
                                #endregion
                                break;

                            case 5:
                                #region Friday
                                if (currentWeek.FridayIn == null)
                                {
                                    currentWeek.FridayIn = date[0];
                                    currentWeek.FridayInID = checkInID;
                                }

                                if (currentWeek.FridayOut == null)
                                {
                                    currentWeek.FridayOut = date[1];
                                    currentWeek.FridayOutID = checkOutID;
                                }

                                currentWeek.FridayDate = date[2];
                                #endregion
                                break;
                        }
                    }
                }
                currentWeek.Week = week.Week;

                if (currentWeek.MondayIn != null || currentWeek.TuesdayIn != null || currentWeek.WednesdayIn != null || currentWeek.ThursdayIn != null || currentWeek.FridayIn != null)
                    OutPutList.Add(currentWeek);

                currentWeek = new OutPut();
            }

            if (OutPutList.Count > 0)
                progressBarDisabled = true;
        }

        /// <summary>
        /// Adds a new check time
        /// </summary>
        /// <returns></returns>
        async Task CreateCheckTime()
        {
            string newDate = newCheckDateTime.ToString().Substring(0, 10);
            string newCheckTime = newCheckDateTime.ToString().Substring(11, 5);
            newCheckDateTime = DateTimeOffset.Parse($"{newDate} {newCheckTime}.00").UtcDateTime;

            string[] newDateArray = new string[3];
            newDateArray[0] = newDate.Substring(6, 4);
            newDateArray[1] = newDate.Substring(3, 2);
            newDateArray[2] = newDate.Substring(0, 2);

            DateTime dateTime = new DateTime(
                Convert.ToInt32(newDateArray[0]),
                Convert.ToInt32(newDateArray[1]),
                Convert.ToInt32(newDateArray[2]));

            string dayOfWeek = (int)dateTime.DayOfWeek switch
            {
                1 => "Monday",
                2 => "Tuesday",
                3 => "Wednesday",
                4 => "Thursday",
                5 => "Friday",
                _ => ""
            };

            var selectedWeekTimes = yearSummeries.FirstOrDefault(x => x.Week == selectedWeek).Times;
            var selectedDayTimes = selectedWeekTimes.FirstOrDefault(x => x.Key == dayOfWeek).Value;

            CheckTime checkTime = new CheckTime
            {
                CardId = studentCard,
                Time = newCheckDateTime,
                Scanner = scannersList.FirstOrDefault(x => x.Name == selectedScanner)
            };

            if (!selectedDayTimes.Any(x => x.Time.AddSeconds(-x.Time.Second) == checkTime.Time) && CanCreateTime)
                await Services.CheckTimes.CreateChecktime(checkTime);

            CloseEditTime();
            await GetChecktimes(studentCard, selectedYear);
        }

        /// <summary>
        /// Updates the check-in and check-out times
        /// </summary>
        /// <returns></returns>
        async Task EditCheckTime()
        {
            foreach (var item in currentCheckTimes)
            {
                if (item.OldCheckTime != item.CurrentCheckTime)
                {
                    if (item.CurrentCheckTime == new DateTimeOffset())
                        await Services.CheckTimes.DeleteChecktime(item.CheckTimeID);
                    else
                    {
                        string checkInTime = item.CurrentCheckTime.ToString().Substring(11, 5);
                        item.CurrentCheckTime = DateTimeOffset.Parse($"{selectedDate} {checkInTime}.00");

                        item.CurrentCheckTime = item.CurrentCheckTime.UtcDateTime;

                        var selectedCheckTime = new CheckTime
                        {
                            CardId = studentCard,
                            ID = item.CheckTimeID,
                            Time = item.CurrentCheckTime
                        };

                        await Services.CheckTimes.UpdateChecktime(selectedDateCheckInID, selectedCheckTime);
                    }
                }
            }

            CloseEditTime();
            await GetChecktimes(studentCard, selectedYear);
        }

        List<DailyCheckTime> currentCheckTimes = new();
        class DailyCheckTime
        {
            public long CheckTimeID { get; set; }
            public DateTimeOffset CurrentCheckTime { get; set; }
            public DateTimeOffset OldCheckTime { get; set; }
            public bool checkTimeDisabled { get; set; }
        }

        /// <summary>
        /// Shows the EditTime menu and sets the values in it
        /// </summary>
        /// <param name="date"></param>
        /// <param name="checkInID"></param>
        /// <param name="checkOutID"></param>
        async Task ShowEditTime(int week, string date, long checkInID, long checkOutID)
        {
            selectedDate = date != null ? date : ""; // If date isn't equal to null then use the 'date' variable, otherwise use ""
            selectedWeek = week;
            selectedDateCheckInID = checkInID;
            selectedDateCheckOutID = checkOutID;

            List<CheckTime> weeklyCheckTimes = new();

            if (date != null)
            {
                date = date.Substring(0, 10);  // Date of day

                // Gets the day of the week
                DateTime dateTime = new DateTime(
                    Convert.ToInt32(date.Substring(6, 4)),
                    Convert.ToInt32(date.Substring(3, 2)),
                    Convert.ToInt32(date.Substring(0, 2)));

                var result = await Services.CheckTimes.GetCheckTimes(studentCard, dateTime);
                weeklyCheckTimes = result.Result;
            }

            currentCheckTimes = new List<DailyCheckTime>();

            foreach (var time in weeklyCheckTimes)
            {
                if (time.ID != 0)
                {
                    currentCheckTimes.Add(new DailyCheckTime
                    {
                        CheckTimeID = time.ID,
                        CurrentCheckTime = time.Time.ToLocalTime(),
                        OldCheckTime = time.Time.ToLocalTime(),
                        checkTimeDisabled = false
                    });
                }
                else
                {
                    currentCheckTimes.Add(new DailyCheckTime
                    {
                        checkTimeDisabled = true
                    });
                }
            }

            hideEditTime = false;
        }

        /// <summary>
        /// Closes the EditTime menu and resets all of the values in it
        /// </summary>
        void CloseEditTime()
        {
            selectedDate = "";
            selectedScanner = "";
            selectedWeek = 0;
            selectedDateCheckInID = 0;
            selectedDateCheckOutID = 0;

            newCheckDateTime = DateTimeOffset.Now;

            hideEditTime = true;
        }

        protected override async Task OnInitializedAsync()
        {
            OutPutList = new List<OutPut>();

            selectedYear = DateTime.Now.Year;
            CloseEditTime();

            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
            modelUser = user.ToUser();
            studentCard = modelUser.CardId;
            if (Card != null && Card != "")
            {
                // Gets the user from the card you want to watch
                studentCard = Card;
                var result = await Services.Users.GetUsersFromCards(Card);
                if (result.HasSucceded && result.Result.Count != 0)
                {
                    User = result.Result.FirstOrDefault();
                    IsOtherUser = true;
                    UserIsLoaded = true;
                }
            }
            else
            {
                // Put the current login user as the users time who you see
                User = modelUser;
                UserIsLoaded = true;
            }
            var scannerList = await Services.Scanners.GetScanners();
            scannersList = scannerList.Result.OrderBy(x => x.Name).ToList();

            foreach (var scanner in scannersList)
                scannerNames.Add(scanner.Name);

            await GetChecktimes(studentCard, DateTime.Now.Year);
        }
    }
}