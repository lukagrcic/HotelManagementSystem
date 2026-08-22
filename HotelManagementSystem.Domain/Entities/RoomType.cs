using HotelManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Domain.Entities
{
    public class RoomType
    {
        public int RoomTypeId { get; set; }
        public RoomCategory Category { get; set; }
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
