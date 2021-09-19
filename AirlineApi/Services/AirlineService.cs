using AirlineApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AirlineApi.Services
{
    public class AirlineService
    {
        private readonly IMongoCollection<Airline> _airlines;

        public AirlineService(IAirlineDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _airlines = database.GetCollection<Airline>(settings.AirlineCollectionName);
        }

        public List<Airline> Get()
        {
            return _airlines.Find(airline => true).ToList();
        }

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


        public Airline Get(string id) =>
            _airlines.Find<Airline>(airline => airline.Id == id).FirstOrDefault();

        public Airline Create(Airline airline)
        {
            _airlines.InsertOne(airline);
            return airline;
        }

        public Airline Update(string id, Airline airline) {
            _airlines.ReplaceOne(a => a.Id == id, airline);
            return airline;
        }


        public void Remove(Airline airline) =>
            _airlines.DeleteOne(a => a.Id == airline.Id);

        public void Remove(string id) =>
            _airlines.DeleteOne(a => a.Id == id);
    }
}

public class PageResponse
{
    public PageResponse(List<Airline> Airlines, long TotalAmount)
    {
        this.Airlines = Airlines;
        this.TotalAmount = TotalAmount;
    }

    public List<Airline> Airlines { get; set; }

    public long TotalAmount { get; set; }
}