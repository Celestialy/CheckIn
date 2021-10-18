using CheckIn.Frontend.Helper;
using CheckIn.Frontend.Services;
using CheckIn.Shared.Models;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace CheckIn.Frontend.Pages.RoomPages
{
    public partial class MoveUserToRoom
    {
        [Parameter]
        public int RoomId { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        public IServices Services { get; set; }
        //User to add
        public User SelectedUserAdd { get; set; }
        //user to delete
        public User SelectedUserRemove { get; set; }
        //room with users in
        public RoomWithUsers room { get; set; }
        //all users in department
        public List<User> Students { get; set; }
        public Pagination pagination { get; set; } = new Pagination();
        public string errormsg = "";
        public bool canMoveUserToRoom = false;
        public bool isLoading = false;

        private Timer aTimer;

        protected override async Task OnInitializedAsync()
        {
            //sets current user
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
            var User = user.ToUser();

            //sets timer
            aTimer = new Timer(400);
            aTimer.Elapsed += UserFinishedTypeing; ;
            aTimer.AutoReset = false;

            //sets deafult pagination options
            pagination.Departments = User.Departments.toStringArray();
            pagination.pageSize = 5;
            pagination.currentPage = 1;
            await reloadRoom();
            await reloadUsers();
        }

        private void UserFinishedTypeing(object sender, ElapsedEventArgs e)
        {
            _ = reloadUsers();
        }

        /// <summary>
        /// triggers each time page changes
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        async Task OnPage(MatPaginatorPageEvent e)
        {
            pagination.currentPage = e.PageIndex + 1;
            await reloadUsers();
        }
        /// <summary>
        /// triggers everytime user is selected from table
        /// </summary>
        /// <param name="row"></param>
        public void SelectionChangedEvent(object row)
        {
            if (row == null)
            {
                SelectedUserAdd = new User();
                errormsg = "";
                canMoveUserToRoom = false;
            }
            else
            {
                SelectedUserAdd = row as User;
                if (SelectedUserAdd.CardId != null && !room.Students.Any(x => x.Id == SelectedUserAdd.Id))
                {
                    
                    canMoveUserToRoom = true;
                }
                else
                {
                    if (SelectedUserAdd.CardId == null)
                    {
                        errormsg = "Bruger har ik noget kort";
                    }
                    else
                    {
                        errormsg = "Bruger er allerede i rummet";
                    }
                    canMoveUserToRoom = false;
                }
            }

            this.StateHasChanged();
        }
        /// <summary>
        /// Gets all the users
        /// </summary>
        /// <returns></returns>
        public async Task reloadUsers()
        {
            isLoading = true;
            var result = await Services.Users.GetUsers(pagination);
            Students = result.Result.Items;
            pagination = result.Result.Pagination;
            StateHasChanged();
            isLoading = false;
        }
        /// <summary>
        /// Load the current room
        /// </summary>
        /// <returns></returns>
        public async Task reloadRoom()
        {
            isLoading = true;
            var wrappedroom = await Services.Rooms.GetRoom(RoomId);
            var unwrappedRoom = wrappedroom.Result;
            if (unwrappedRoom.Cards.Count > 0)
            {
                var wrappedstudents = await Services.Users.GetUsersFromCards(unwrappedRoom.Cards);
                var students = wrappedstudents.Result;
                room = new RoomWithUsers(unwrappedRoom, students);
            }
            else
            {
                room = new RoomWithUsers(unwrappedRoom, new List<User>());
            }
            isLoading = false;
        }
        /// <summary>
        /// Searches after user when enter is pressed in search field
        /// </summary>
        /// <param name="e"></param>
        public void Search(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                aTimer.Stop();
                _ = reloadUsers();
            }            
        }
        /// <summary>
        /// opdatere search hver gang man skriver noget
        /// </summary>
        /// <param name="e"></param>
        public void OnInput(ChangeEventArgs e)
        {
            aTimer.Stop();
            pagination.SearchQuery = e.Value.ToString();
            aTimer.Start();
        }


        /// <summary>
        ///  moves user to the room
        /// </summary>
        /// <returns></returns>
        public async Task moveUserToRoom()
        {
            if (canMoveUserToRoom)
            {
                await Services.Rooms.MoveCardToRoom(RoomId, SelectedUserAdd.CardId);
                await reloadUsers();
                await reloadRoom();
            }
            else
            {
                await Task.Delay(1);
            }
        }
        /// <summary>
        /// Removes the user from the room
        /// </summary>
        /// <returns></returns>
        public async Task RemoveUserFromRoom()
        {
            if (SelectedUserRemove != null)
            {
                await Services.Rooms.RemoveCardFromRoom(RoomId, SelectedUserRemove.CardId);
                await reloadUsers();
                await reloadRoom();
                SelectedUserRemove = null;
            }
            else
            {
                await Task.Delay(1);
            }
        }

    }
}
