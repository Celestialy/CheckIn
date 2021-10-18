using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CheckIn.Shared.Models.Types;
using Microsoft.Extensions.Logging;

namespace CheckIn.Shared.Models
{
    public class Logging
    {
        /// <summary>
        /// Log Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Log message
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// Sets the level of the log
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public LogLevel Level { get; set; }
        /// <summary>
        /// Set what entity that has been logged
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public LogType Type { get; set; }
        /// <summary>
        /// Sets the action that was performed
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public LogAction Action { get; set; }
        /// <summary>
        /// Sets the time of the log to 
        /// </summary>
        public DateTimeOffset LoggedAt { get; set; } = DateTime.Now;

    }
}
