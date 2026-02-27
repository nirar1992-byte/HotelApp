using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Room
    {
        //Properties
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public int ExtraBeds { get; set; }
        public bool IsAvailable { get; set; }
        public List <Booking> Bookings { get; set; } = new List<Booking>();



        //Konstruktor
        public Room(string roomName,string roomType, int extraBeds)
        {
           RoomName = roomName;
            RoomType = roomType;
            ExtraBeds = extraBeds;
            IsAvailable = true;
        }

        // Databas konstruktor
        public Room() 
        {
        }



    }
}
