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
    public class App
    {
        public void Build(DbContextOptionsBuilder<HotelDbContext> options)
        {
            bool programRunning = true;
            do
            {
                MainMenu.ShowMenu(options);
            } while (programRunning);
        }
    }
}
