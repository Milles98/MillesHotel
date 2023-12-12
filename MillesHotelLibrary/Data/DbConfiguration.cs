using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Data
{
    public static class DbConfiguration
    {
        public static DbContextOptionsBuilder<HotelDbContext> StartDatabase()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var connectionString = config.GetConnectionString("MillesHotelContextConnection");

            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var dbContext = new HotelDbContext(optionsBuilder.Options))
            {
                dbContext.Database.Migrate();
                DbSeeding.SeedData(dbContext);
            }

            return optionsBuilder;
        }
    }
}
