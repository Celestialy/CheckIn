using CheckIn.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CheckIn.Frontend.Services.SignalR
{
    /// <summary>
    /// Handels signalr
    /// </summary>
    public class CheckinHub
    {
        /// <summary>
        /// Holds the connection
        /// </summary>
        private HubConnection hubConnection;
        /// <summary>
        /// Invokes when recieving incomming checktimes
        /// </summary>
        public event Action<CheckTime> OnUpdate;
        /// <summary>
        /// Invokes when receiving incomming Cards
        /// </summary>
        public event Action<CardScanned> OnCardCardScanned;

        /// <summary>
        /// Instanciate SignalR Connection
        /// </summary>
        /// <param name="Token"></param>
        public CheckinHub(string Token)
        {
            hubConnection = new HubConnectionBuilder().WithUrl($"{Settings.API_URL}chekinhub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(Token);
            }).Build();
            hubConnection.On<CheckTime>("UpdateCheckTime", (checktime) => OnUpdate?.Invoke(checktime));
            hubConnection.On<CardScanned>("CardScanned", (card) => OnCardCardScanned?.Invoke(card));
            hubConnection.StartAsync();
        }

        /// <summary>
        /// Instanciate SignalR Connection
        /// </summary>
        /// <param name="Token"></param>
        public CheckinHub()
        {
            hubConnection = new HubConnectionBuilder().WithUrl($"{Settings.API_URL}chekinhub").Build();
            hubConnection.On<CardScanned>("CardScanned", (card) => OnCardCardScanned?.Invoke(card));
            hubConnection.StartAsync();
        }
        /// <summary>
        /// Disposes the connection when the class isnt in use anymore
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            //await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
            
        }
    }
}
