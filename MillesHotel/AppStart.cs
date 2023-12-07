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
            //Skapa en klass för configurationen nedan
            //en klass för dataseeding
            //kalla på dom i denna
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var connectionString = config.GetConnectionString("MillesHotelContextConnection");
            var options = new DbContextOptionsBuilder<HotelDbContext>();
            options.UseSqlServer(connectionString);

            using (var dbContext = new HotelDbContext(options.Options))
            {
                dbContext.Database.Migrate();
            }

            bool programRunning = true;
            do
            {
                MainMenu.ShowMenu();
            } while (programRunning);
        }
    }
}
