using CheckIn.Shared.IHubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Hubs
{
    public class CheckInHub : Hub<ICheckInHub>
    {
        // activates on new client connecting
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Public");
            if (Context.User.Identity.IsAuthenticated)
            {
                // gets cardid from user and check if it can add it a group for the user and card
                var cardid = Context.User.FindFirst("cardid").Value;
                if (cardid != "")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.FindFirst("cardid").Value);
                }
                // check if user has one of these roles if they have then they will be added to that group also 
                if (Context.User.IsInRole("Administrator"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Administrator");
                }
                if (Context.User.IsInRole("Instructor"))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Instructor");
                }
            }
            

            await base.OnConnectedAsync();
        }

        //[Authorize(Roles = "Administrator")]
        //public async Task AddToGroup(string groupName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //}
    }
}
