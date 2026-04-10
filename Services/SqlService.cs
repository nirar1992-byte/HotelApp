using System;
using Microsoft.Data.SqlClient;

namespace HotelApp.Services
{
    public class SqlService
    {
        private string connectionString = "Server=(localdb)\\mssqllocaldb;Database=HotelDb;Trusted_Connection=True;";

        // 1. Hämta alla rum
        public void GetAllRooms()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Rooms";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Room ID: {reader["Id"]}");
                }
            }
        }

        // 2. Hämta lediga rum
        public void GetAvailableRooms()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Rooms WHERE IsAvailable = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Available Room ID: {reader["Id"]}");
                }
            }
        }

        // 3. Räkna antal rum
        public void CountRooms()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM Rooms";

                SqlCommand cmd = new SqlCommand(query, conn);
                int count = (int)cmd.ExecuteScalar();

                Console.WriteLine($"Total rooms: {count}");
            }
        }

        // 4. JOIN (bonus – visar högre nivå)
        public void GetRoomBookings()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT Rooms.Id AS RoomId, Bookings.Id AS BookingId
                    FROM Rooms
                    JOIN Bookings ON Rooms.Id = Bookings.RoomId";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Room: {reader["RoomId"]}, Booking: {reader["BookingId"]}");
                }
            }
        }
    }
}