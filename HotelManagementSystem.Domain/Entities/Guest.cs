namespace HotelManagementSystem.Domain.Entities
{
    public class Guest
    {
        public int GuestId { get; set; }
        public string Jmbg { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int CityId { get; set; }
        public City City { get; set; } = null!;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}