using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Shared.Models
{
    /// <summary>
    /// Model used for check in/out times
    /// </summary>
    public class CheckTime
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        /// <summary>
        /// Check in/out time
        /// </summary>
        public DateTimeOffset Time { get; set; }


        /// <summary>
        /// The card that checked this time
        /// </summary>
        public string CardId { get; set; }


        /// <summary>
        /// The scanner that checked this time
        /// </summary>
        public Scanner Scanner { get; set; }
    }
}
