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
    public partial class ScannerManagement
    {
        [Inject]
        public IServices Services { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        bool createScannerDialog = false, editScannerDialog = false, deleteScannerDialog = false;

        string newScannerName = "";
        string newScannerMacAddress = "";
        Scanner selectedScanner;
        List<Scanner> scannersList = new List<Scanner>();

        async Task GetScanners()
        {
            var scannerList = await Services.Scanners.GetScanners();
            scannersList = scannerList.Result.OrderBy(x => x.Name).ToList();
        }

        async Task CreateScanner()
        {
            DateTimeOffset today = DateTimeOffset.Now;

            Scanner newScanner = new Scanner
            {
                Added = today,
                Name = newScannerName,
                MacAddress = newScannerMacAddress
            };

            await Services.Scanners.CreateScanner(newScanner);
            createScannerDialog = false;
            await GetScanners();
        }

        async Task EditScanner()
        {
            selectedScanner.Name = newScannerName;
            await Services.Scanners.EditScanner(selectedScanner.MacAddress, selectedScanner);
            editScannerDialog = false;
            await GetScanners();
        }

        async Task DeleteScanner()
        {
            await Services.Scanners.DeleteScanner(selectedScanner.MacAddress);
            deleteScannerDialog = false;
            await GetScanners();
        }

        protected override async Task OnInitializedAsync()
        {
            selectedScanner = new Scanner();
            await GetScanners();
        }
    }
}