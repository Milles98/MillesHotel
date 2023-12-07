using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MillesHotel.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel
{
    public class AppStart
    {
        public void Run()
        {

            DbConfiguration.StartDatabase();

            bool programRunning = true;
            do
            {
                MainMenu.ShowMenu();
            } while (programRunning);
        }
    }
}
