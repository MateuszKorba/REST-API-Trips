using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi4.Models.DTO.Requests;
using WebApi4.Services;

namespace WebApi4.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private IDataBaseService _dataBaseService;

        public TripsController(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var reusultOperation = await _dataBaseService.GetTrips();
            return Ok(reusultOperation);
        }

        [HttpPost("{IdTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip([FromBody] TripsRequestDto tripsRequestDto, [FromRoute] int IdTrip) {
            var reusultOperation = await _dataBaseService.AssignAClientToTrip(tripsRequestDto,IdTrip);
            if (reusultOperation.StatusCode == 400)
            {
                return BadRequest(reusultOperation.StatusDescription);
            }
            else if (reusultOperation.StatusCode == 404)
            {
                return NotFound(reusultOperation.StatusDescription);
            }
            else {
                return Ok(reusultOperation.StatusDescription);
            }
        }
    }
}
