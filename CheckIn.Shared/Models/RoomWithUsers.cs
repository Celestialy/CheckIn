using System;
using System.Collections.Generic;
using System.Text;

namespace CheckIn.Shared.Models
{
    public class RoomWithUsers : Room
    {
        public List<User> Students { get; set; }

        public int StudentCount { get { return this.Students.Count; } }

        public RoomWithUsers()
        {
            
        }
        public RoomWithUsers(Room room, List<User> students)
        {
            this.ID = room.ID;
            this.Added = room.Added;
            this.Cards = room.Cards;
            this.Department = room.Department;
            this.RoomName = room.RoomName;
            this.Scanner = room.Scanner;
            this.Students = students;
        }
    }
}
