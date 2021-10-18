using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CheckIn.Shared.IHubs
{
    public interface ICheckInHub
    {
        /// <summary>
        /// Sends new card to listners
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        Task CardScanned(CardScanned card);
        /// <summary>
        /// Sends an update of new or updated checktime to listners
        /// </summary>
        /// <param name="checkTime"></param>
        /// <returns></returns>
        Task UpdateCheckTime(CheckTime checkTime);

    }
}
