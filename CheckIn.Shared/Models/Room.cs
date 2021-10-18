using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Shared.Models
{

    public class Room
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Name of room
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// Department this room belongs to
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Cards that are in this room
        /// </summary>
        public List<Card> Cards { get; set; }
        /// <summary>
        /// Scanner that belongs to this room
        /// </summary>
        public Scanner Scanner { get; set; }

        /// <summary>
        /// Time this room was created
        /// </summary>
        public DateTimeOffset Added { get; set; }
    }
}
