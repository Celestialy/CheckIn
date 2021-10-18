using CheckIn.Frontend.Wrappers;
using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Services
{
    public interface IScanners
    {
        /// <summary>
        /// Gets a list of scanners
        /// </summary>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="List{T}"/> of type <see cref="Scanner"/></returns>
        public Task<APIWrapper<List<Scanner>>> GetScanners();

        /// <summary>
        /// Gets scanner from id
        /// </summary>
        /// <param name="id">Scanner Id</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="Scanner"/></returns>
        public Task<APIWrapper<Scanner>> GetScanner(string id);

        /// <summary>
        /// Create's a new scanner
        /// </summary>
        /// <param name="scanner">Scanner you want to create</param>
        /// <returns>An <see cref="APIWrapper{T}"/> of type <see cref="Scanner"/></returns>
        public Task<APIWrapper<Scanner>> CreateScanner(Scanner scanner);

        /// <summary>
        /// Delete's a scanner
        /// </summary>
        /// <param name="id">Scanner id: <see cref="Scanner.MacAddress"/></param>
        /// <returns></returns>
        public Task<APIWrapper<bool>> DeleteScanner(string id);

        /// <summary>
        /// Edit's a scanner
        /// </summary>
        /// <param name="id">Scanner Mac Address</param>
        /// <param name="scanner">Scanner to edit</param>
        /// <returns></returns>
        public Task<APIWrapper<Scanner>> EditScanner(string id, Scanner scanner);
    }
}