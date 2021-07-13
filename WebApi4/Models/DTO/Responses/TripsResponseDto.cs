using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi4.Models.DTO.Responses
{
    public class TripsResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public IEnumerable<dynamic> Countries { get; set; }
        public IEnumerable<dynamic> Clients { get; set; }
    }
}
