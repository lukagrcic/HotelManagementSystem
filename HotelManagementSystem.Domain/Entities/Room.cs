using HotelManagementSystem.Domain.Enums;

namespace HotelManagementSystem.Domain.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public int Capacity { get; set; }
        public RoomStatus Status { get; set; }

        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;

        public ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();
    }
}