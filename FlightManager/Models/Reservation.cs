using FlightManager.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace FlightManager.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Second name required")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }
        [StringLength(10, ErrorMessage = "PersonalID \"{0}\" must have {2} character", MinimumLength = 10)]
        public string PersonalID { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Nationality required")]
        public string Nationality { get; set; }
        [Required(ErrorMessage = "Ticket type required")]
        public string TicketType { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        public int FlightId { get; set; }
    }
}
