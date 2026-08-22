using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Domain.Entities
{
    public class ReservationRoom
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;


        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

    }
}
