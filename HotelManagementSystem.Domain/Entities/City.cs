using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Domain.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;

        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    }
}
