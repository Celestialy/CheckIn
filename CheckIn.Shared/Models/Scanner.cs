using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Shared.Models
{
    /// <summary>
    /// Model used for allowed scanners
    /// </summary>
    public class Scanner
    {
        /// <summary>
        /// Scanners macaddress
        /// </summary>
        [Key]
        public string MacAddress { get; set; }

        /// <summary>
        /// Name of this scanner
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Time this scanner was created
        /// </summary>
        public DateTimeOffset Added { get; set; }

        ///// <summary>
        ///// Room that belongs to scanner
        ///// </summary>
        //public Room Room { get; set; }
    }
}
