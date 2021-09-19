using AirlineApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AirlineApi.Services
{
    public class AirlineService
    {
        /** The airline collection of the MongoDB */
        private readonly IMongoCollection<Airline> _airlines;

        public AirlineService(IAirlineDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _airlines = database.GetCollection<Airline>(settings.AirlineCollectionName);
        }

        /**
         * Retrieve all airlines.
         */
        public List<Airline> Get()
        {
            return _airlines.Find(airline => true).ToList();
        }

        /**
         * Retrieve a set amount of airlines.
         * <param name="page">The page to retrieve data for</param>
         * <param name="filterValue">The name of the airline on which should be filtered</param>
         * <param name="amount">The amount of airlines to retrieve</param>
         */
        public PageResponse Get(int page, int amount, string filterValue = "")
        {
            // Filter dataset.
            var filteredAirlines = _airlines.Find(airline => airline.Name.ToLower().Contains(filterValue.ToLower()));
            var totalAmountOfAirlines = filteredAirlines.ToList().Count;

            // Set page data.
            var amountToSkip = page * amount;
            var airlinesToReturn = filteredAirlines.Skip(amountToSkip).Limit(amount).ToList();

            return new PageResponse(airlinesToReturn, totalAmountOfAirlines);
        }

        /**
         * Retrieve one airline.
         * <param name="id">The unique identifier</param>
         */
        public Airline Get(string id) =>
            _airlines.Find<Airline>(airline => airline.Id == id).FirstOrDefault();

        /**
         *  Create a new airline.
         *  <param name="airline">The airline to create</param>
         */
        public Airline Create(Airline airline)
        {
            _airlines.InsertOne(airline);
            return airline;
        }

        /**
         * Update the given airline.
         * <param name="id">The unique identifier</param>
         * <param name="airline">The new airline</param>
         */
        public Airline Update(string id, Airline airline) {
            _airlines.ReplaceOne(a => a.Id == id, airline);
            return airline;
        }

        /**
         * Delete an airline.
         * <param name="airline">The airline to remove</param>
         */
        public void Remove(Airline airline) =>
            _airlines.DeleteOne(a => a.Id == airline.Id);

        /**
         * Delete an airline based on the id.
         * <param name="id">The unique identifier</param>
         */
        public void Remove(string id) =>
            _airlines.DeleteOne(a => a.Id == id);
    }
}

/**
 * The API response for a page.
 */
public class PageResponse
{
    public PageResponse(List<Airline> Airlines, long TotalAmount)
    {
        this.Airlines = Airlines;
        this.TotalAmount = TotalAmount;
    }

    /** The airlines to send. */
    public List<Airline> Airlines { get; set; }

    /** The total amount of airlines. */
    public long TotalAmount { get; set; }
}