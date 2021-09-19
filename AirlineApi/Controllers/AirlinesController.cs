using AirlineApi.Models;
using AirlineApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirlineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirlinesController : ControllerBase
    {
        private readonly AirlineService _airlineService;

        public AirlinesController(AirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpGet]
        public ActionResult<PageResponse> Get(int page, int amount, string filterValue = "")
        {
            var airlineResponse = _airlineService.Get(page, amount, filterValue);

            if (airlineResponse.Airlines == null)
            {
                return NotFound();
            }

            return airlineResponse;
        }

        [HttpGet("{id:length(24)}", Name = "GetAirline")]
        public ActionResult<Airline> Get(string id)
        {
            var airline = _airlineService.Get(id);

            if (airline == null)
            {
                return NotFound();
            }

            return airline;
        }

        [HttpPost]
        public ActionResult<Airline> Create(Airline airline)
        {
            _airlineService.Create(airline);

            return CreatedAtRoute("GetAirline", new { id = airline.Id.ToString() }, airline);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult<Airline> Update(string id, Airline airlineIn)
        {
            var airline = _airlineService.Get(airlineIn.Id);

            if (airline == null)
            {
                return NotFound();
            }

           return _airlineService.Update(airlineIn.Id, airlineIn);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var airline = _airlineService.Get(id);

            if (airline == null)
            {
                return NotFound();
            }

            _airlineService.Remove(airline.Id);

            return NoContent();
        }
    }
}
