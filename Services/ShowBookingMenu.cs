using HotelApp.Models;
using HotelApp.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelApp.Services
{
    internal class ShowBookingMenu
    {
        public static void ShowBookingMenuMethod(HotelManager hotelManager, ApplicationDbContext dbContext)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("|\t1. Skapa ny bokning          |");
            Console.WriteLine("|\t2. Ändra bokning             |");
            Console.WriteLine("|\t3. Ta bort bokning           |");
            Console.WriteLine("|\t4. Återvänd till Huvudmeny   |");
            Console.WriteLine("======================================");

            int choice;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Välj alternativ (1-4): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 4)
                    break;

                Console.WriteLine("Välj en siffra mellan 1 - 4.");
            }

            switch (choice)
            {
                case 1:

                    Console.WriteLine("\nBefintliga Kunder:");
                    var customers = dbContext.Customers.ToList();

                    foreach (var c in customers)
                        Console.WriteLine($"- Kund ID: {c.CustomerId}, Namn: {c.Name}, Email: {c.Email}, Tel: {c.PhoneNumber}");

                    Console.Write("\nAnge kund ID: ");
                    int customerId;
                    string? inputCustomerId = Console.ReadLine();

                    while (!int.TryParse(inputCustomerId, out customerId) ||
                           !customers.Any(c => c.CustomerId == customerId))
                    {
                        Console.Write("Fel: Ange giltigt kund-ID: ");
                        inputCustomerId = Console.ReadLine();
                    }

                    Console.WriteLine("\nBefintliga Rum:");
                    var rooms = dbContext.Rooms.ToList();

                    foreach (var r in rooms)
                        Console.WriteLine($"- Rum ID: {r.RoomId}, Namn: {r.RoomName}, Typ: {r.RoomType}, Extrasängar: {r.ExtraBeds}");

                    Console.Write("\nAnge rum ID: ");
                    int roomId;
                    string? inputRoomId = Console.ReadLine();

                    while (!int.TryParse(inputRoomId, out roomId) ||
                           !rooms.Any(r => r.RoomId == roomId))
                    {
                        Console.Write("Fel: Ange giltigt rums-ID: ");
                        inputRoomId = Console.ReadLine();
                    }

                    Console.Write("\nAnge startdatum (YYYY-MM-DD): ");
                    DateTime startDate;
                    string? inputDate = Console.ReadLine();

                    while (!DateTime.TryParse(inputDate, out startDate) ||
                           startDate < DateTime.Today)
                    {
                        Console.Write("Ange giltigt framtida datum: ");
                        inputDate = Console.ReadLine();
                    }

                    Console.Write("\nAnge antal nätter: ");
                    int nights;
                    string? inputNights = Console.ReadLine();

                    while (!int.TryParse(inputNights, out nights) || nights <= 0)
                    {
                        Console.Write("Ange giltigt antal nätter: ");
                        inputNights = Console.ReadLine();
                    }

                    hotelManager.MakeBooking(customerId, roomId, startDate, nights);
                    break;

                case 2:

                    Console.Write("\nAnge namn på kund vars bokning ska ändras: ");
                    string? nameOfBooker = Console.ReadLine()?.ToLower();

                    var allCustomers = dbContext.Customers.ToList();

                    while (string.IsNullOrWhiteSpace(nameOfBooker) ||
                           !allCustomers.Any(c => c.Name.ToLower() == nameOfBooker))
                    {
                        Console.Write("Ange giltigt namn: ");
                        nameOfBooker = Console.ReadLine()?.ToLower();
                    }

                    var selectedCustomer =
                        allCustomers.First(c => c.Name.ToLower() == nameOfBooker);

                    var customerBookings = dbContext.Bookings
                        .Include(b => b.BookedRoom)
                        .Where(b => b.CustomerId == selectedCustomer.CustomerId)
                        .ToList();

                    if (!customerBookings.Any())
                    {
                        Console.WriteLine("Kunden har inga bokningar.");
                        return;
                    }

                    Console.WriteLine("\nBokningar:");
                    for (int i = 0; i < customerBookings.Count; i++)
                    {
                        var b = customerBookings[i];
                        Console.WriteLine($"{i + 1}. Rum: {b.BookedRoom.RoomName}, {b.StartDate:d} - {b.EndDate:d}");
                    }

                    Console.Write("\nVälj bokning: ");
                    int chosenBooking;
                    string? inputChoice = Console.ReadLine();

                    while (!int.TryParse(inputChoice, out chosenBooking) ||
                           chosenBooking < 1 ||
                           chosenBooking > customerBookings.Count)
                    {
                        Console.Write("Felaktigt val: ");
                        inputChoice = Console.ReadLine();
                    }

                    var selectedBooking = customerBookings[chosenBooking - 1];

                    Console.Write("\nAnge nytt startdatum (YYYY-MM-DD): ");
                    DateTime newStart;
                    while (!DateTime.TryParse(Console.ReadLine(), out newStart) ||
                           newStart < DateTime.Today)
                    {
                        Console.Write("Felaktigt datum: ");
                    }

                    Console.Write("Ange antal nätter: ");
                    int newNights;
                    while (!int.TryParse(Console.ReadLine(), out newNights) ||
                           newNights <= 0)
                    {
                        Console.Write("Felaktigt antal: ");
                    }

                    DateTime newEnd = newStart.AddDays(newNights);

                    var otherBookings = dbContext.Bookings
                        .Where(b => b.RoomId == selectedBooking.RoomId &&
                                    b.BookingId != selectedBooking.BookingId)
                        .ToList();

                    bool overlap =
                        otherBookings.Any(b => b.IsOverlapping(newStart, newEnd));

                    if (overlap)
                    {
                        Console.WriteLine("Datumet krockar med annan bokning.");
                    }
                    else
                    {
                        selectedBooking.StartDate = newStart;
                        selectedBooking.EndDate = newEnd;
                        dbContext.SaveChanges();
                        Console.WriteLine("Bokning uppdaterad.");
                    }

                    break;

                case 3:

                    hotelManager.ShowBookings();
                    Console.Write("\nAnge rumsnamn för bokning som ska tas bort: ");
                    string? roomName = Console.ReadLine();

                    while (string.IsNullOrWhiteSpace(roomName))
                    {
                        Console.Write("Ange giltigt rumsnamn: ");
                        roomName = Console.ReadLine();
                    }

                    var bookingsForRoom = dbContext.Bookings
                        .Include(b => b.BookedRoom)
                        .Include(b => b.Customer)
                        .Where(b => b.BookedRoom.RoomName == roomName)
                        .ToList();

                    if (!bookingsForRoom.Any())
                    {
                        Console.WriteLine("Inga bokningar hittades.");
                        return;
                    }

                    for (int i = 0; i < bookingsForRoom.Count; i++)
                    {
                        var b = bookingsForRoom[i];
                        Console.WriteLine($"{i + 1}. {b.Customer.Name}, {b.StartDate:d}-{b.EndDate:d}");
                    }

                    Console.Write("Välj bokning att ta bort: ");
                    int deleteChoice;
                    while (!int.TryParse(Console.ReadLine(), out deleteChoice) ||
                           deleteChoice < 1 ||
                           deleteChoice > bookingsForRoom.Count)
                    {
                        Console.Write("Felaktigt val: ");
                    }

                    dbContext.Bookings.Remove(bookingsForRoom[deleteChoice - 1]);
                    dbContext.SaveChanges();
                    Console.WriteLine("Bokning borttagen.");

                    break;

                case 4:
                    return;
            }
        }
    }
}