using System;

namespace Wonk
{
    public class Banner
    {
        public static void PrintBanner()
        {
            // team colors
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Welcome to Wonk...\nStarting up now...", Console.ForegroundColor, Console.BackgroundColor);
        }
    }
}
