using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Services
{
    public class Pause
    {
        public class PauseMethod
        {

            // Metod som har i syfte att pausa
            public static void Pause()
            {
                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }

        }

    }
}
