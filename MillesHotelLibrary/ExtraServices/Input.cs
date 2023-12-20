using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.ExtraServices
{
    public static class Input
    {
        public static T GetUserInput<T>(string prompt)
        {
            Console.Write(prompt);
            string userInput = Console.ReadLine();

            try
            {
                return (T)Convert.ChangeType(userInput, typeof(T));
            }
            catch (FormatException)
            {
                Message.ErrorMessage("Unavailable choice, please try again.");
            }
            catch (InvalidCastException)
            {
                Message.ErrorMessage("Unavailable conversion, please try again.");
            }

            return default(T);
        }
    }
}
