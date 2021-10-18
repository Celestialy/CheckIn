using CheckIn.Frontend.Services;
using CheckIn.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Frontend.Wrappers;
using Microsoft.AspNetCore.Components.Authorization;
using CheckIn.Frontend.Helper;

namespace CheckIn.Frontend.Pages.RoomPages
{
    public partial class RoomUsersOverview
    {
        [Parameter]
        public int RoomId { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IServices Services { get; set; }
        RoomWithUsers Room { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        List<StudentStatus> students = new();

        protected override async Task OnInitializedAsync()
        {
            var wrappedroom = await Services.Rooms.GetRoom(RoomId);
            var unwrappedRoom = wrappedroom.Result;
            if (unwrappedRoom.Cards.Count > 0)
            {
                var wrappedstudents = await Services.Users.GetUsersFromCards(unwrappedRoom.Cards);
                
                students = new List<StudentStatus>();
                foreach (var item in wrappedstudents.Result)
                {
                    // Gets the check times for all of the users in the room
                    var result = await Services.CheckTimes.GetCheckTimes(item.CardId, DateTime.UtcNow);
                    var checkTimes = result.Result;

                    string checkInStatus = "";
                    if (checkTimes.Count == 0) // User hasn't checked in yet
                        checkInStatus = "studentNotCheckIn"; // Red
                    else if (DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Friday && checkTimes.Count % 2 == 0)
                    {
                        if (GetActiveCheckInTime(checkTimes) >= 5) // User didn't check out too early
                            checkInStatus = "studentCheckOut"; // Blue

                        else if (GetActiveCheckInTime(checkTimes) < 5) // User checked out too early
                            checkInStatus = "studentCheckOutTooEarly"; // Yellow
                    }
                    else if ((DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Monday ||
                            DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Tuesday ||
                            DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Wednesday ||
                            DateTimeOffset.UtcNow.DayOfWeek == DayOfWeek.Thursday) &&
                            checkTimes.Count % 2 == 0)
                    {
                        if (GetActiveCheckInTime(checkTimes) >= 8) // User didn't check out too early
                            checkInStatus = "studentCheckOut"; // Blue
                        else if (GetActiveCheckInTime(checkTimes) < 8) // User checked out too early
                            checkInStatus = "studentCheckOutTooEarly"; // Yellow
                    }
                    else if (checkTimes.Count > 0) // User has checked in
                        checkInStatus = "studentCheckIn"; // Green

                    students.Add(new StudentStatus
                    {
                        Name = item.Name,
                        CardId = item.CardId,
                        Status = checkInStatus
                    });
                }

                Room = new RoomWithUsers(unwrappedRoom, wrappedstudents.Result);
            }
            else
                Room = new RoomWithUsers(unwrappedRoom, new List<User>());

        }

        void ManageRoom()
        {
            NavigationManager.NavigateTo($"/roomoverview/moveusertoroom/{RoomId}");
        }

        void NavigateUserAnnualOverview(string card)
        {
            NavigationManager.NavigateTo($"/annualOverview/{card}");
        }

        class StudentStatus
        {
            public string Name { get; set; }
            public string CardId { get; set; }
            public string Status { get; set; }
        }

        /// <summary>
        /// Gets the actual time you have been checked in
        /// </summary>
        /// <param name="checkTimes"></param>
        /// <returns>Formatted string with the actual time</returns>
        double GetActiveCheckInTime(List<CheckTime> checkTimes)
        {
            TimeSpan totaleTime = new();
            for (int row = 0; row < checkTimes.Count - 1; row++)
            {
                if (row % 2 == 0)
                    totaleTime += checkTimes[row + 1].Time - checkTimes[row].Time;
                else
                    totaleTime -= checkTimes[row + 1].Time - checkTimes[row].Time;
            }

            return totaleTime.TotalHours;
        }
    }
}