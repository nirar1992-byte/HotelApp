using System;
using Microsoft.Data.SqlClient;

namespace HotelApp.Services
{
    public class SqlService
    {
        public string connectionString = "Server=(localdb)\\mssqllocaldb;Database=HotelDb;Trusted_Connection=True;";

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
                    Console.WriteLine($"Room ID: {reader["RoomId"]}");
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
                    Console.WriteLine($"Available Room ID: {reader["RoomId"]}");
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

        // 4. JOIN mellan Rooms och Bookings
        public void GetRoomBookings()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT Rooms.RoomId, Bookings.BookingId
                    FROM Rooms
                    JOIN Bookings ON Rooms.RoomId = Bookings.RoomId";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Room: {reader["RoomId"]}, Booking: {reader["BookingId"]}");
                }
            }
        }

        // 5. ORDER BY 
        public void GetRoomsOrdered()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Rooms ORDER BY RoomName ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Room: {reader["RoomName"]}");
                }
            }
        }
    }
}