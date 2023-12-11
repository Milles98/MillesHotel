using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Services
{
    public class RoomService
    {
        private readonly HotelDbContext _dbContext;

        public RoomService(DbContextOptionsBuilder<HotelDbContext> options)
        {
            _dbContext = new HotelDbContext(options.Options);
        }

        public void CreateRoom()
        {
        }

        public void GetRoomByID()
        {
        }

        public void UpdateRoom()
        {

        }

        public void DeleteRoom()
        {

        }

    }
}
