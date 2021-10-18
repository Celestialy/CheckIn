using CheckIn.Frontend.Helper;
using CheckIn.Frontend.Services;
using CheckIn.Frontend.Services.SignalR;
using CheckIn.Shared.Models;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace CheckIn.Frontend.Pages.AdminPages
{
    public partial class CardsScanned : IAsyncDisposable
    {
        [Inject]
        public IServices Services { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        //holds all recived cards
        private List<CardScanned> Cards { get; set; } = new List<CardScanned>();
        //holds all cards currently displayed
        public List<CardScanned> FilteretCards { get; set; } = new List<CardScanned>();
        public bool ToggleToUnknownCards { get; set; } = false;
        public CardScanned SelectedCard { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public Pagination pagination { get; set; } = new Pagination();

        CheckinHub hub;

        public User SelectedUser { get; set; } = new User();

        public bool Openmordal = false;

        public bool isLoading = true;

        public bool canLinkUser = false;

        public bool canOpenModal = false;

        private Timer aTimer;

        protected override async Task OnInitializedAsync()
        {
            //gets access token and then start a signalr connection
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            accessTokenResult.TryGetToken(out var token);
            hub = new CheckinHub(token.Value);
            hub.OnCardCardScanned += Hub_OnCardCardScanned;

            //sets timer
            aTimer = new Timer(400);
            aTimer.Elapsed += UserFinishedTypeing; ;
            aTimer.AutoReset = false;

            //sets current user
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
            var User = user.ToUser();
            pagination.Departments = User.Departments.toStringArray();
            pagination.pageSize = 5;
            pagination.currentPage = 1;
            await reloadUsers();

        }

        private void UserFinishedTypeing(object sender, ElapsedEventArgs e)
        {
            _ = reloadUsers();
        }

        /// <summary>
        /// Gets invoked each time a card gets send with signalR
        /// </summary>
        /// <param name="card"></param>
        private void Hub_OnCardCardScanned(CardScanned card)
        {
            Cards.Add(card);
            if (ToggleToUnknownCards && card.isNewCard)
            {
                FilteretCards.Add(card);
            }
            if (!ToggleToUnknownCards)
            {
                FilteretCards.Add(card);
            }
            StateHasChanged();
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
        /// filters the list so it only shows cards that doesnt have a user
        /// </summary>
        /// <param name="e"></param>
        public void filterForUnkownCards(bool e)
        {
            ToggleToUnknownCards = e;
            if (ToggleToUnknownCards)
                FilteretCards = Cards.Where(x => x.isNewCard == true).ToList();
            else
                FilteretCards = Cards;
        }

        /// <summary>
        /// Gets all the users
        /// </summary>
        /// <returns></returns>
        public async Task reloadUsers()
        {
            isLoading = true;
            var result = await Services.Users.GetUsers(pagination);
            Users = result.Result.Items;
            pagination = result.Result.Pagination;
            isLoading = false;
            StateHasChanged();
        }

        /// <summary>
        /// Invoked each time a card is selected in the UI
        /// </summary>
        /// <param name="row"></param>
        public void SelectionChangedEvent(object row)
        {
            if (row == null)
            {
                SelectedCard = new CardScanned();
                canOpenModal = false;
            }
            else
            {
                SelectedCard = row as CardScanned;
                if (SelectedCard.card != null)
                    canOpenModal = true;
                else
                    canOpenModal = false;
            }

            this.StateHasChanged();
        }

        /// <summary>
        /// triggers everytime user is selected from table
        /// </summary>
        /// <param name="row"></param>
        public void UserSelected(object row)
        {
            if (row == null)
            {
                SelectedUser = new User();
                canLinkUser = false;
            }
            else
            {
                SelectedUser = row as User;
                if (SelectedCard.card != null)
                    canLinkUser = true;
                else
                    canLinkUser = false;
            }

            this.StateHasChanged();
        }

        /// <summary>
        /// Links a cardto a user
        /// </summary>
        /// <returns></returns>
        public async Task LinkCardToUser()
        {
            if (canLinkUser)
            {
                var result = await Services.Users.AddCardToUser(SelectedCard.card._card, SelectedUser.Id);
                if (result.HasSucceded)
                {
                    SelectedCard.Username = SelectedUser.Name;
                    SelectedCard.isNewCard = false;
                    await Services.CheckTimes.ChangeCard(SelectedCard.card._card, SelectedUser.CardId);
                    StateHasChanged();
                }
                Openmordal = false;
            }
        }

        /// <summary>
        /// Deletes the selected checktime
        /// </summary>
        /// <returns></returns>
        public async Task DeleteChecktime()
        {
            if (canOpenModal)
            {
                var result = await Services.CheckTimes.DeleteChecktime(SelectedCard.ChecktimeId);
                if (result.HasSucceded)
                {
                    Cards.Remove(SelectedCard);
                    FilteretCards.Remove(SelectedCard);
                }
            }

        }

        /// <summary>
        /// Invoked when you change page on the user table
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        async Task OnPage(MatPaginatorPageEvent e)
        {
            pagination.currentPage = e.PageIndex + 1;
            await reloadUsers();
        }

        /// <summary>
        /// Invoked when you change page to stop the signalr connection
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            hub.OnCardCardScanned -= Hub_OnCardCardScanned;
            await hub.DisposeAsync();

        }


    }
}
