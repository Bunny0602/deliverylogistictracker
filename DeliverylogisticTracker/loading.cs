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
            Console.WriteLine("════════════════════════════════════════════");
            Console.WriteLine("║       Logistics Tracker System           ║");
            Console.WriteLine("════════════════════════════════════════════");
            Console.ResetColor();

            Console.WriteLine("\nLoading System!!!\n");

            string loadingBar = "--------------------";
            char[] barArray = loadingBar.ToCharArray();

            Console.Write("[");

            for (int i = 0; i < barArray.Length; i++)
            {
                barArray[i] = '█'; 
                Console.Write(barArray[i]);
                Thread.Sleep(130); 
            }

            Console.Write("]");
            Thread.Sleep(700);
            Console.Clear();
        }

    }
}
