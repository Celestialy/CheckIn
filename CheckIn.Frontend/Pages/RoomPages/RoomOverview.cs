using CheckIn.Frontend.Helper;
using CheckIn.Frontend.Services;
using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Pages.RoomPages
{
    public partial class RoomOverview
    {
        bool progressBarDisabled = false;
        [Inject]
        public IServices Services { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        List<MatRefWrapper<RoomWithUsers>> Rooms { get; set; } = new List<MatRefWrapper<RoomWithUsers>>();

        User User;
        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
            User = user.ToUser();
            var deps = User.Departments.toStringArray();
            var WrappedRooms = await Services.Rooms.GetRoomsByDepartments(deps);
            foreach (var room in WrappedRooms.Result)
            {
                RoomWithUsers roomsWithUsers;
                if (room.Cards.Count > 0)
                {
                    var wrappedstudents = await Services.Users.GetUsersFromCards(room.Cards);
                    var students = wrappedstudents.Result;
                    roomsWithUsers = new RoomWithUsers(room, students);
                }
                else
                {
                    roomsWithUsers = new RoomWithUsers(room, new List<User>());
                }

                Rooms.Add(new MatRefWrapper<RoomWithUsers>(roomsWithUsers));
            }
            progressBarDisabled = true;
        }

        void OnClick(MatButton button, MatMenu menu) => menu.OpenAsync(button.Ref);

        void NavigateRoomUserOverview(int roomId)
        {
            navigationManager.NavigateTo($"/roomoverview/room/{roomId}");
        }

        void CreateRoom()
        {
            navigationManager.NavigateTo("/roomoverview/create");
        }
        void NavigateUserAnnualOverview(string card)
        {
            navigationManager.NavigateTo($"/annualOverview/{card}");
        }
    }
}
