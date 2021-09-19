namespace AirlineApi.Models
{
    public class AirlineDatabaseSettings : IAirlineDatabaseSettings
    {
        public string AirlineCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IAirlineDatabaseSettings
    {
        string AirlineCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}