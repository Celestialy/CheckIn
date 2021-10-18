using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    /// <inheritdoc/>
    public interface IYearSummeries
    {
        /// <summary>
        /// Gets yearsummery from user
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Task<APIWrapper<List<YearSummery>>> GetYearSummeries(string cardId, int year);
    }
}
