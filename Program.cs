using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HotelApp.Context;
using HotelApp.Models;
using HotelApp.Services;
using static HotelApp.Services.Pause;

namespace HotelApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Hämta konfiguration från appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Skapa DbContextOptions
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Skapa DbContext
            using var dbContext = new ApplicationDbContext(contextOptions);

            // Kör migrationer
            dbContext.Database.Migrate();

            // Skapa HotelManager
            HotelManager hotelManager = new HotelManager(dbContext);

            // ✅👇 LÄGG TILL DETTA BLOCK HÄR
            var sqlService = new SqlService();

            sqlService.GetAllRooms();
            sqlService.GetAvailableRooms();
            sqlService.CountRooms();
            sqlService.GetRoomBookings();
            // ✅👆 SLUT PÅ DITT TILLÄGG

            bool runProgram = true;

            while (runProgram)
            {
                ShowMainMenu.ShowMainMenuMethod();

                int choice;

                while (true)
                {
                    Console.Write("Välj ett alternativ (1-5): ");
                    string? input = Console.ReadLine();

                    if (int.TryParse(input, out choice) && choice >= 1 && choice <= 5)
                        break;

                    Console.WriteLine("Vänligen välj en siffra mellan 1 - 5.");
                    Console.WriteLine();
                }

                switch (choice)
                {
                    case 1:
                        ShowRoomMenu.ShowRoomMenuMethod(hotelManager, dbContext);
                        break;

                    case 2:
                        ShowCustomerMenu.ShowCustomerMenuMethod(hotelManager, dbContext);
                        break;

                    case 3:
                        ShowBookingMenu.ShowBookingMenuMethod(hotelManager, dbContext);
                        break;

                    case 4:
                        AvailabilityMenu.AvailabilityMenuMethod(hotelManager, dbContext);
                        break;

                    case 5:
                        runProgram = false;
                        break;
                }

                PauseMethod.Pause();
            }
        }
    }
}