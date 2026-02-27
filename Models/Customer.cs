using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HotelApp.Models

{
   
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }



        // Konstruktor
        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            PhoneNumber = phone;
        }
       
        public Customer()
        {
        }
    }
}