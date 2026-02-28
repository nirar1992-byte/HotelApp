using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HotelApp.Models
{
    // Boknings Klassen
    public class Booking
    {
        public int BookingId { get; set; }                                      
        public int CustomerId { get; set; } 
        public int RoomId { get; set; } 

        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public Models.Customer Customer { get; set; } 
        public Models.Room BookedRoom { get; set; }
        
        private static int nextBookingId = 1;
                                      
        
        
        
        // Konstruktorrr
        public Booking(DateTime start, DateTime end, Models.Customer customer, Models.Room
        bookedRoom)
        {
            RoomId = bookedRoom.RoomId; 
            CustomerId = customer.CustomerId; 
            StartDate = start;
            EndDate = end;
            Customer = customer;
            BookedRoom = bookedRoom;
        }
        
     
        public Booking()
        {
        }



        // Metod: Kontrollera om bokningen överlappar en annan bokning
        public bool IsOverlapping(DateTime start, DateTime end)
        {
            // Returnerar true om bokningarna överlappar, annars false
            return (start < EndDate && end > StartDate);
        }

    }
}