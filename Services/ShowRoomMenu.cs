using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Context;
using HotelApp.Models;

namespace HotelApp.Services
{
    public class ShowRoomMenu
    {
        public static void ShowRoomMenuMethod(HotelManager hotelManager, ApplicationDbContext dbContext)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("|\t1. Lägg till nytt rum        |");
            Console.WriteLine("|\t2. Ändra rumsuppgifter       |");
            Console.WriteLine("|\t3. Ta bort rum               |");
            Console.WriteLine("|\t4. Återvänd till Huvudmeny   |");
            Console.WriteLine("======================================");
            Console.WriteLine();

            int choice;

            while (true)
            {
                Console.Write("Välj ett alternativ (1-4): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 4)
                    break;

                Console.WriteLine("Vänligen välj en siffra mellan 1 - 4.");
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine();
                    Console.WriteLine("1. Enkelrum");
                    Console.WriteLine("2. Dubbelrum liten (60 kvm)");
                    Console.WriteLine("3. Dubbelrum stor (100 kvm)");
                    Console.Write("Välj rumstyp (1-3): ");

                    string? inputRoom = Console.ReadLine();
                    int roomChoice;

                    while (!int.TryParse(inputRoom, out roomChoice) || roomChoice < 1 || roomChoice > 3)
                    {
                        Console.Write("Välj 1 - 3: ");
                        inputRoom = Console.ReadLine();
                    }

                    string roomType = roomChoice == 1 ? "Enkelrum" : "Dubbelrum";

                    int maxExtraBeds = 0;
                    if (roomChoice == 2) maxExtraBeds = 1;
                    if (roomChoice == 3) maxExtraBeds = 2;

                    int extraBeds = 0;

                    if (maxExtraBeds > 0)
                    {
                        Console.Write($"Hur många extrasängar (0 - {maxExtraBeds}): ");
                        string? bedInput = Console.ReadLine();

                        while (!int.TryParse(bedInput, out extraBeds) || extraBeds < 0 || extraBeds > maxExtraBeds)
                        {
                            Console.Write($"Ange 0 - {maxExtraBeds}: ");
                            bedInput = Console.ReadLine();
                        }
                    }

                    Console.Write("Ange rumsnummer (ex 105): ");
                    string? roomName = Console.ReadLine();

                    while (roomName == null || dbContext.Rooms.Any(r => r.RoomName == roomName))
                    {
                        Console.Write("Ogiltigt eller redan existerande rum. Ange nytt: ");
                        roomName = Console.ReadLine();
                    }

                    Room newRoom = new Room(roomName, roomType, extraBeds);
                    dbContext.Rooms.Add(newRoom);
                    dbContext.SaveChanges();

                    Console.WriteLine($"Rum {roomName} skapades.");
                    break;

                case 2:
                    Console.Write("Ange rumsnummer att ändra: ");
                    string? editInput = Console.ReadLine();

                    var roomToEdit = dbContext.Rooms.FirstOrDefault(r => r.RoomName == editInput);

                    while (roomToEdit == null)
                    {
                        Console.Write("Rummet hittades inte. Försök igen: ");
                        editInput = Console.ReadLine();
                        roomToEdit = dbContext.Rooms.FirstOrDefault(r => r.RoomName == editInput);
                    }

                    Console.WriteLine("1. Ändra rumsnummer");
                    Console.WriteLine("2. Ändra rumstyp");
                    Console.WriteLine("3. Ändra extrasängar");
                    Console.Write("Välj (1-3): ");

                    string? editChoiceInput = Console.ReadLine();
                    int editChoice;

                    while (!int.TryParse(editChoiceInput, out editChoice) || editChoice < 1 || editChoice > 3)
                    {
                        Console.Write("Välj 1 - 3: ");
                        editChoiceInput = Console.ReadLine();
                    }

                    switch (editChoice)
                    {
                        case 1:
                            Console.Write("Nytt rumsnummer: ");
                            string? newRoomName = Console.ReadLine();

                            while (newRoomName == null || dbContext.Rooms.Any(r => r.RoomName == newRoomName))
                            {
                                Console.Write("Ogiltigt eller upptaget. Ange nytt: ");
                                newRoomName = Console.ReadLine();
                            }

                            roomToEdit.RoomName = newRoomName;
                            break;

                        case 2:
                            Console.Write("Ny rumstyp (Enkelrum/Dubbelrum): ");
                            string? newType = Console.ReadLine();

                            while (newType == null ||
                                  (newType.ToLower() != "enkelrum" && newType.ToLower() != "dubbelrum"))
                            {
                                Console.Write("Ange Enkelrum eller Dubbelrum: ");
                                newType = Console.ReadLine();
                            }

                            roomToEdit.RoomType = newType;
                            break;

                        case 3:
                            Console.Write("Nytt antal extrasängar (0-2): ");
                            string? bedInputEdit = Console.ReadLine();
                            int newBeds;

                            while (!int.TryParse(bedInputEdit, out newBeds) || newBeds < 0 || newBeds > 2)
                            {
                                Console.Write("Ange 0 - 2: ");
                                bedInputEdit = Console.ReadLine();
                            }

                            roomToEdit.ExtraBeds = newBeds;
                            break;
                    }

                    dbContext.Rooms.Update(roomToEdit);
                    dbContext.SaveChanges();

                    Console.WriteLine("Rummet uppdaterades.");
                    break;

                case 3:
                    Console.Write("Ange rumsnummer att ta bort: ");
                    string? deleteInput = Console.ReadLine();

                    var roomToDelete = dbContext.Rooms.FirstOrDefault(r => r.RoomName == deleteInput);

                    while (roomToDelete == null)
                    {
                        Console.Write("Rummet hittades inte. Försök igen: ");
                        deleteInput = Console.ReadLine();
                        roomToDelete = dbContext.Rooms.FirstOrDefault(r => r.RoomName == deleteInput);
                    }

                    dbContext.Rooms.Remove(roomToDelete);
                    dbContext.SaveChanges();

                    Console.WriteLine("Rummet har tagits bort.");
                    break;

                case 4:
                    ReturnToMainMenu.ReturnToMainMenuMethod();
                    break;
            }
        }

    }
}
