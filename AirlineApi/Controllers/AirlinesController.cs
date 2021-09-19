using AirlineApi.Models;
using AirlineApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirlineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirlinesController : ControllerBase
    {
        /**
         * Reference to the airline service. 
         */
        private readonly AirlineService _airlineService;

        public AirlinesController(AirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        /** 
         * Get the airlines for a specific page.
         * <param name="amount">The amount of airlines to retrieve</param>
         * <param name="filterValue">The name of the airline to filter the data on</param>
         * <param name="page">The current page to retrieve data for</param>
         */
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

        /**
         * Retrieve one airlne.
         * <param name="id">The unique identifier of an airline</param>
         */
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

        /**
         * Create a new airline.
         * <param name="airline">The airline to create</param>
         */
        [HttpPost]
        public ActionResult<Airline> Create(Airline airline)
        {
            _airlineService.Create(airline);

            return CreatedAtRoute("GetAirline", new { id = airline.Id.ToString() }, airline);
        }

        /**
         * Update the passed airline with the new values.
         * <param name="airlineIn">The updated airline</param>
         * <param name="id">The unique identifier of the airline</param>
         */
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

        /**
         * Remove the airline with the given id.
         * <param name="id">The unique identifier of the airline</param>
         */
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
