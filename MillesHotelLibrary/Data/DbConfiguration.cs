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
        public static HotelDbContext StartDatabase()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<HotelDbContext>();
            var connectionString = config.GetConnectionString("MillesHotelContextConnection");
            options.UseSqlServer(connectionString);

            var dbContext = new HotelDbContext(options.Options);
            var dbSeeding = new DbSeeding();
            dbSeeding.SeedData(dbContext);

            return dbContext;
        }
    }
}
