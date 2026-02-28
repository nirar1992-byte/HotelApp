
using System;
using System.Collections.Generic;
using System.Linq;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Context
{
    public class HotelManager
    {
        private readonly ApplicationDbContext _dbContext;

        public HotelManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!_dbContext.Customers.Any() && !_dbContext.Rooms.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer("Nirar Yosefe", "nirar.yosefe@gmail.com", "076 000 00 00"),
                    new Customer("Anna Zachrisson","anna.z@gmail.com","073 333 53 65"),
                    new Customer("Patrik Taraksson", "patte.t@yahoo.com","076 444 52 87"),
                    new Customer("Mohammed Ibn Halal", "mohalle.muzmatch@yahoo.com","072 458 32 65")
                };

                _dbContext.Customers.AddRange(customers);

                var rooms = new List<Room>
                {
                    new Room("1001", "Enkelrum", 0),
                    new Room("1002", "Dubbelrum", 1),
                    new Room("1003", "Dubbelrum", 2),
                    new Room("1004", "Enkelrum", 0)
                };

                _dbContext.Rooms.AddRange(rooms);

                _dbContext.SaveChanges();
            }
        }








        // Metoder för att hjälpa till sköta bokningar,kunder etc
        public void AddRoom(Room room)
        {
            _dbContext.Rooms.Add(room);
            _dbContext.SaveChanges();

            Console.WriteLine($"Rum {room.RoomName} har lagts till.");
        }

        public void AddCustomer(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            Console.WriteLine($"Kund {customer.Name} har lagts till.");
        }

        public void ShowRooms()
        {
            var rooms = _dbContext.Rooms.ToList();

            Console.WriteLine("Lista över alla rum:");
            foreach (var room in rooms)
            {
                Console.WriteLine($"- ID: {room.RoomId}, Namn: {room.RoomName}, Typ: {room.RoomType}, Extrasängar: {room.ExtraBeds}, Ledigt: {(room.IsAvailable ? "Ja" : "Nej")}");
            }
        }

        public void ShowCustomers()
        {
            var customers = _dbContext.Customers.ToList();

            Console.WriteLine("Lista över alla kunder:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"- ID: {customer.CustomerId}, Namn: {customer.Name}, Email: {customer.Email}, Telefon: {customer.PhoneNumber}");
            }
        }

        public void ShowBookings()
        {
            var bookings = _dbContext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.BookedRoom)
                .ToList();

            Console.WriteLine("Lista över alla bokningar:");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"- Bokad rum: {booking.BookedRoom.RoomName}, bookare: {booking.Customer.Name}, startdatum: {booking.StartDate.ToShortDateString()}, slutdatum: {booking.EndDate.ToShortDateString()}");
            }
        }

        public void MakeBooking(int customerId, int roomId, DateTime startDate, int nights)
        {
            if (nights <= 0)
            {
                Console.WriteLine("Antal nätter måste vara minst 1.");
                return;
            }

            if (startDate < DateTime.Today)
            {
                Console.WriteLine("Du kan inte boka ett rum för ett datum som redan har passerat.");
                return;
            }

            var room = _dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
            var customer = _dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (room == null || customer == null)
            {
                Console.WriteLine("Fel: Rummet eller kunden existerar inte.");
                return;
            }

            DateTime endDate = startDate.AddDays(nights);

            bool isBooked = _dbContext.Bookings.Any(b =>
                b.RoomId == roomId &&
                startDate < b.EndDate &&
                endDate > b.StartDate);

            if (isBooked)
            {
                Console.WriteLine("Rummet är redan bokat under denna period.");
                return;
            }

            var booking = new Booking
            {
                StartDate = startDate,
                EndDate = endDate,
                CustomerId = customerId,
                RoomId = roomId
            };

            _dbContext.Bookings.Add(booking);
            _dbContext.SaveChanges();

            room.IsAvailable = false;
            _dbContext.Rooms.Update(room);
            _dbContext.SaveChanges();

            Console.WriteLine($"Bokning skapad! {customer.Name} har bokat {room.RoomName} från {startDate.ToShortDateString()} till {endDate.ToShortDateString()}.");
        }
    }

}