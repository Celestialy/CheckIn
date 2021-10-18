using CheckIn.Frontend.Helper;
using CheckIn.Frontend.Services;
using CheckIn.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Pages.RoomPages
{
    public partial class CreateRoom
    {
        [Inject]
        public IServices Services { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        //room to add
        public Room Room { get; set; } = new Room();
        //room to delete
        public Room SelectedRoomToRemove { get; set; }
        // all rooms from users departments
        public List<Room> Rooms { get; set; }
        public Scanner SelectedScanner { get; set; }
        public List<Scanner> Scanners { get; set; }
        public List<string> Departments { get; set; }
        //public bool hasScanner { get { return (Room.Scanner != null); } }
        //public bool hasName { get { return (Room.RoomName != null || Room.RoomName != ""); } }
        //public bool hasDepartment { get { return (Room.Department != null || Room.Department != ""); } }
        public bool readyToCreate { get { return (Room.RoomName != null || Room.RoomName != "" || Room.Scanner != null || Room.Department != null || Room.Department != ""); } }
        protected override async Task OnInitializedAsync()
        {
            var wrappedScanners = await Services.Scanners.GetScanners();
            Scanners = wrappedScanners.Result;
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
            var User = user.ToUser();
            var deps = User.Departments.toStringArray();
            Departments = deps.ToList();
            var WrappedRooms = await Services.Rooms.GetRoomsByDepartments(deps);
            Rooms = WrappedRooms.Result;
        }

        public async Task createRoom()
        {
            if (readyToCreate)
            {
                var result = await Services.Rooms.CreateRoom(Room);
                if (result.HasSucceded)
                    navigationManager.NavigateTo("/roomoverview");
            }
            
        }

        public async Task removeRoom()
        {
            if (SelectedRoomToRemove != null)
            {
                var result = await Services.Rooms.DeleteRoom(SelectedRoomToRemove.ID);
                if (result.HasSucceded)
                    navigationManager.NavigateTo("/roomoverview");
            }

        }
    }
}
