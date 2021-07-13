using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi4.Models.DTO.Requests
{
    public class TripsRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }
        public string TripName { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
