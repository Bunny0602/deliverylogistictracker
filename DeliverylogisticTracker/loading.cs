using System;
using System.Threading;

namespace DeliverylogisticTracker
{
    class loading
    {
        public static void loadingScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n══════════════════════════════════════════");
            Console.WriteLine("║       🚛 Logistics Tracker System       ║");
            Console.WriteLine("══════════════════════════════════════════");
            Console.ResetColor();

            Console.WriteLine("\nInitializing system... Please wait.\n");

            string loadingBar = "--------------------";
            char[] barArray = loadingBar.ToCharArray();

            Console.Write("[");

            for (int i = 0; i < barArray.Length; i++)
            {
                barArray[i] = '█'; 
                Console.Write(barArray[i]);
                Thread.Sleep(500); 
            }

            Console.Write("] Complete");
            Thread.Sleep(1500);
            Console.Clear();
        }

    }
}
