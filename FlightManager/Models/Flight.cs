using FlightManager.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Location from required")]
        public string LocationFrom { get; set; }
        [Required(ErrorMessage = "Location to required")]
        public string LocationTo { get; set; }
        [Required(ErrorMessage = "Departure date required")]
        public DateTime DepartureDate { get; set; }
        [Required(ErrorMessage = "Arrival date required")]
        public DateTime ArrivalDate { get; set; }
        [Required(ErrorMessage = "Plane type required")]
        public string PlaneType { get; set; }
        [Required(ErrorMessage = "Plane id required")]
        public string PlaneId { get; set; }
        [Required(ErrorMessage = "Pilot name required")]
        public string PilotName { get; set; }
        [Required(ErrorMessage = "Available seats required")]
        public int AvailableSeats { get; set; }
        [Required(ErrorMessage = "Available seats business class required")]
        public int AvailableSeatsBusinessClass { get; set; }

        [DefaultValue(null)]
        public virtual ICollection<Reservation>? Reservations { get; set; }

        //public int PageNumber { get; set; } = 1;
        //public int PageSize { get; set; } = 10;
        public TimeSpan Duration => ArrivalDate - DepartureDate;
    }
}
