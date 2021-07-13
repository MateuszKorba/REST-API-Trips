using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApi4.Models;
using WebApi4.Models.DTO.Requests;
using WebApi4.Models.DTO.Responses;

namespace WebApi4.Services
{
    public interface IDataBaseService
    {
        public Task<ICollection<TripsResponseDto>> GetTrips();
        public Task<HttpStatusCodeResult> DeleteClientData(int IdClient);
        public Task<HttpStatusCodeResult> AssignAClientToTrip(TripsRequestDto tripsRequestDto, int IdTrip);
    }  
}
