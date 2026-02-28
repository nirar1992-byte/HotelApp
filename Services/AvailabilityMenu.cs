using System;
using HotelApp.Context;
using HotelApp.Models;

namespace HotelApp.Services
{
    public class AvailabilityMenu
    {
        public static void AvailabilityMenuMethod(HotelManager hotelManager, ApplicationDbContext dbContext)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("|\t                             |");
            Console.WriteLine("|\t1. Visa alla bokningar       |");
            Console.WriteLine("|\t2. Visa alla rum             |");
            Console.WriteLine("|\t3. Visa alla kunder          |");
            Console.WriteLine("|\t4. Återvänd till Huvudmeny   |");
            Console.WriteLine("|\t                             |");
            Console.WriteLine("======================================");

            int choice;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Välj ett alternativ (1-4): ");

                string? input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 4)
                {
                    break;
                }

                Console.WriteLine("Vänligen välj en siffra från menyn 1 - 4.");
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine();
                    hotelManager.ShowBookings();
                    break;

                case 2:
                    Console.WriteLine();
                    hotelManager.ShowRooms();
                    break;

                case 3:
                    Console.WriteLine();
                    hotelManager.ShowCustomers();
                    break;

                case 4:
                    return;
            }
        }
    }
}