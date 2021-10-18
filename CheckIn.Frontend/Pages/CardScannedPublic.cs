using CheckIn.Frontend.Services.SignalR;
using CheckIn.Shared.Models;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Shared.Helpers;

namespace CheckIn.Frontend.Pages
{
    public partial class CardScannedPublic
    {
        [Parameter]
        public string MacAddress { get; set; }
        CheckinHub hub;
        protected override void OnInitialized()
        {
            hub = new CheckinHub();
            hub.OnCardCardScanned += Hub_OnCardCardScanned;

        }

        private void Hub_OnCardCardScanned(CardScanned card)
        {
            if (card.scanner.MacAddress == MacAddress)
                if (card.IsCheckingIn)
                {
                    Toaster.Add($"Godmorgen {card.Username}", MatToastType.Success, "Checked ind" );
                }
                else
                {
                    if (card.Time.DayOfWeek == DayOfWeek.Friday)
                        if (card.CheckedInTime.Hours >= 5)
                            Toaster.Add($"Nu det fyraften for {card.Username}!", MatToastType.Success, "Checked ind");
                        else
                            Toaster.Add($"Hov hov er du sikker på {card.Username} har arbejdet nok du har kun været her i {card.CheckedInTime.ToReadableTime()}", MatToastType.Danger, "Checked ud");
                    else
                        if (card.CheckedInTime.Hours >= 8)
                            Toaster.Add($"Nu det fyraften for {card.Username}!", MatToastType.Success, "Checked ind");
                        else
                            Toaster.Add($"Hov hov er du sikker på {card.Username} har arbejdet nok du har kun været her i {card.CheckedInTime.ToReadableTime()}", MatToastType.Danger, "Checked ud");
                }
        }
    }
}
