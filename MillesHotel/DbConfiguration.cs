using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel
{
    public static class DbConfiguration
    {
        public static void StartDatabase()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();

            var connectionString = config.GetConnectionString("MillesHotelContextConnection");

            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var dbContext = new HotelDbContext(optionsBuilder.Options))
            {
                dbContext.Database.Migrate();
            }
        }

        //Lägg till dataseeding metod skapa rum och kunder kanske bokningar idk
    }
}
