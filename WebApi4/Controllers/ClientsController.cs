using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi4.Services;

namespace WebApi4.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private IDataBaseService _dataBaseService;

        public ClientsController(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [HttpDelete("{IdClient}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int IdClient)
        {
            var resultOperation = await _dataBaseService.DeleteClientData(IdClient);
            if (resultOperation.StatusCode == 400)
            {
                return BadRequest(resultOperation.StatusDescription);
            }
            else if (resultOperation.StatusCode == 404)
            {
                return NotFound(resultOperation.StatusDescription);
            }
            else
            {
                return Ok(resultOperation.StatusDescription);
            }
        }
    }
}
