using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Shared.Models
{
    /// <summary>
    /// Model used for scanners POST.
    /// These are the expected props from the scanner
    /// </summary>
    public class ScannerPost
    {
        /// <summary>
        /// The card that was scanned
        /// </summary>
        public string cardID { get; set; }

        /// <summary>
        /// The macaddress of the scanner
        /// </summary>
        public string macAddress { get; set; }
    }
}
