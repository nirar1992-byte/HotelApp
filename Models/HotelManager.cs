using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Models
{
    public class HotelManager
    {
        private ApplicationDbContext_dbContext;

        public HotelManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if(!_dbContext.Customers.Any() & amp; &amp; !_dbContext.Rooms.Any())
            {
                var customers = new List<Customer>
                {
                    // Seed kunder
                    new Customer("Nirar Yosefe", "nirar.yosefe@gmail.com", "076 000 00 00"),
                    new Customer("Anna Zachrisson","anna.z@gmail.com","073 333 53 65"),
                    new Customer("Patrik Taraksson", "patte.t@yahoo.com","076 444 52 87"),
                    new Customer("Mohammed Ibn Halal", "mohalle.muzmatch@yahoo.com","072 458 32 65"),
                    _dbContext.Customers.AddRange(customers);


                    // Seed Rum
                    var rooms = new List<Room>
                    {
                        new Room("1001", "Enkelrum", 0),
                        new Room("1002", "Dubbelrum", 1),
                        new Room("1003", "Dubbelrum", 2),
                        new Room("1004", "Enkelrum", 0)

                    };
                    _dbContext.Rooms.AddRange(rooms);

                    // SPARA TILL DATABAS
                    _dbContext.SaveChanges();
                };
               
            }
        }

    }
}
