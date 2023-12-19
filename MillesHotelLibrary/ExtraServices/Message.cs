using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.ExtraServices
{
    public static class Message
    {
        public static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void InputSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void MillesHotelMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(" __  __ _____ _      _      ______  _____   _    _  ____ _______ ______ _");
            Console.WriteLine("|  \\/  |_   _| |    | |    |  ____|/ ____| | |  | |/ __ \\__   __|  ____| |");
            Console.WriteLine("| \\  / | | | | |    | |    | |__  | (___   | |__| | |  | | | |  | |__  | |");
            Console.WriteLine("| |\\/| | | | | |    | |    |  __|  \\___ \\  |  __  | |  | | | |  |  __| | |");
            Console.WriteLine("| |  | |_| |_| |____| |____| |____ ____) | | |  | | |__| | | |  | |____| |____");
            Console.WriteLine("|_|  |_|_____|______|______|______|_____/  |_|  |_|\\____/  |_|  |______|______|");
            Console.ResetColor();
        }
    }
}
