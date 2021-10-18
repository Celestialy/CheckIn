using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace CheckIn.Shared.Models
{
    /// <summary>
    /// Model for RFID cards
    /// </summary>
    public class Card 
    {
        /// <summary>
        /// Card Id
        /// </summary>
        [Key]
        public string _card { get; set; }
        /// <summary>
        /// Contains the room the card is in
        /// </summary>
        [JsonIgnore]
        public Room Room { get; set; }
        /// <summary>
        /// Id of the room the card is in
        /// </summary>
        public int RoomId { get; set; }
        public Card()
        {

        }
        private Card(string card)
        {
            _card = card;
        }
        /// <summary>
        /// Does so you can implictly convert string to card
        /// </summary>
        /// <param name="card"></param>
        public static implicit operator Card(string card)
        {
            if (card == null)
                return null;
            return new Card(card);
        }
        /// <summary>
        /// Does so you can implictly convert card to string
        /// </summary>
        /// <param name="card"></param>
        public static implicit operator string(Card card)
        {
            if (card == null)
                return null;
            return card._card;
        }
        /// <summary>
        /// returns card id when used in a string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{_card}";
    }
}
