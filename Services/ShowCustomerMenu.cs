using HotelApp.Models;
using HotelApp.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HotelApp.Services
{
    internal class ShowCustomerMenu
    {
        public static void ShowCustomerMenuMethod(HotelManager hotelManager, ApplicationDbContext dbContext)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("|\t1. Lägg till ny kund         |");
            Console.WriteLine("|\t2. Ändra kunduppgifter       |");
            Console.WriteLine("|\t3. Ta bort kund              |");
            Console.WriteLine("|\t4. Återvänd till Huvudmeny   |");
            Console.WriteLine("======================================");

            int choice;

            while (true)
            {
                Console.Write("\nVälj alternativ (1-4): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 4)
                    break;

                Console.WriteLine("Välj en siffra mellan 1 - 4.");
            }

            switch (choice)
            {
                case 1:

                    Console.Write("\nAnge kundens namn: ");
                    string? newCustomerName = Console.ReadLine();

                    while (newCustomerName == null || newCustomerName == "" ||
                           dbContext.Customers.Any(c => c.Name == newCustomerName))
                    {
                        Console.Write("Ogiltigt namn / Kund finns redan. Ange nytt namn: ");
                        newCustomerName = Console.ReadLine();
                    }

                    Console.Write("Ange kundens e-post: ");
                    string? newCustomerMail = Console.ReadLine();
                    while (newCustomerMail == null || newCustomerMail == "")
                    {
                        Console.Write("Ange giltig e-post: ");
                        newCustomerMail = Console.ReadLine();
                    }

                    Console.Write("Ange kundens telefonnummer: ");
                    string? newCustomerNumber = Console.ReadLine();
                    while (newCustomerNumber == null || newCustomerNumber == "")
                    {
                        Console.Write("Ange giltigt telefonnummer: ");
                        newCustomerNumber = Console.ReadLine();
                    }

                    var newCustomer = new Customer(newCustomerName, newCustomerMail, newCustomerNumber);
                    dbContext.Customers.Add(newCustomer);
                    dbContext.SaveChanges();

                    Console.WriteLine($"\nKunden {newCustomerName} har lagts till.");
                    break;

                case 2:

                    Console.Write("\nAnge kundnamn att ändra: ");
                    string? customerToEditName = Console.ReadLine();

                    var customerToEdit =
                        dbContext.Customers.FirstOrDefault(c => c.Name == customerToEditName);

                    while (customerToEditName == null || customerToEditName == "" || customerToEdit == null)
                    {
                        Console.Write("Ogiltigt namn. Försök igen: ");
                        customerToEditName = Console.ReadLine();
                        customerToEdit =
                            dbContext.Customers.FirstOrDefault(c => c.Name == customerToEditName);
                    }

                    Console.WriteLine("\n1. Ändra namn");
                    Console.WriteLine("2. Ändra mail");
                    Console.WriteLine("3. Ändra telefon");

                    Console.Write("Välj (1-3): ");
                    int userChoice;
                    while (!int.TryParse(Console.ReadLine(), out userChoice) ||
                           userChoice < 1 || userChoice > 3)
                    {
                        Console.Write("Välj 1-3: ");
                    }

                    switch (userChoice)
                    {
                        case 1:
                            Console.Write("Nytt namn: ");
                            string? newName = Console.ReadLine();
                            while (newName == null || newName == "" ||
                                   dbContext.Customers.Any(c => c.Name == newName))
                            {
                                Console.Write("Ogiltigt / redan taget namn: ");
                                newName = Console.ReadLine();
                            }

                            customerToEdit.Name = newName;
                            dbContext.SaveChanges();
                            Console.WriteLine("Namn uppdaterat.");
                            break;

                        case 2:
                            Console.Write("Ny e-post: ");
                            string? newMail = Console.ReadLine();
                            while (newMail == null || newMail == "")
                            {
                                Console.Write("Ogiltig e-post: ");
                                newMail = Console.ReadLine();
                            }

                            customerToEdit.Email = newMail;
                            dbContext.SaveChanges();
                            Console.WriteLine("E-post uppdaterad.");
                            break;

                        case 3:
                            Console.Write("Nytt telefonnummer: ");
                            string? newPhone = Console.ReadLine();
                            while (newPhone == null || newPhone == "")
                            {
                                Console.Write("Ogiltigt nummer: ");
                                newPhone = Console.ReadLine();
                            }

                            customerToEdit.PhoneNumber = newPhone;
                            dbContext.SaveChanges();
                            Console.WriteLine("Telefon uppdaterad.");
                            break;
                    }

                    break;

                case 3:

                    Console.Write("\nAnge kundnamn att ta bort: ");
                    string? customerBeingDeleted = Console.ReadLine();

                    while (customerBeingDeleted == null || customerBeingDeleted == "" ||
                           !dbContext.Customers.Any(c => c.Name == customerBeingDeleted))
                    {
                        Console.Write("Ogiltigt namn. Försök igen: ");
                        customerBeingDeleted = Console.ReadLine();
                    }

                    var customerDeleted =
                        dbContext.Customers.First(c => c.Name == customerBeingDeleted);

                    dbContext.Customers.Remove(customerDeleted);
                    dbContext.SaveChanges();

                    Console.WriteLine($"Kunden {customerBeingDeleted} har tagits bort.");
                    break;

                case 4:
                    return;
            }
        }
    }
}