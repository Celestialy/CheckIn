using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CheckIn.Shared.Models
{
    public class CardScanned
    {
        public long ChecktimeId { get; set; }
        /// <summary>
        /// The scanned card
        /// </summary>
        public Card card { get; set; }
        /// <summary>
        /// What scanner it came from
        /// </summary>
        public Scanner scanner { get; set; }
        /// <summary>
        /// if its a new card
        /// </summary>
        public bool isNewCard { get; set; } = false;
        /// <summary>
        /// Time card is scanned
        /// </summary>
        public DateTimeOffset Time { get; set; } = DateTime.Now;
        /// <summary>
        /// Has the time the card was scanned
        /// </summary>
        public DateTimeOffset CheckinTime { get; set; }
        /// <summary>
        /// Has the username of who has the card
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// check if the cards is currently getting checked in or our
        /// </summary>
        public bool IsCheckingIn { get; set; }

        /// <summary>
        /// Gets a string thats says if you are checked in or out
        /// </summary>
        [JsonIgnore]
        public string CheckInMessage
        {
            get
            {
                if (IsCheckingIn)
                    return "Checked ind";
                else
                    return "Checked ud";
            } 
        }
        /// <summary>
        /// Gets the timespan the card has been checked in
        /// </summary>
        [JsonIgnore]
        public TimeSpan CheckedInTime
        {
            get
            {
                return Time - CheckinTime;
            }
        }

    }
}
