using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AirlineApi.Models
{
    public class Airline
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("slogan")]
        public string Slogan { get; set; }

        [BsonElement("logo")]
        public string Logo { get; set; }

        [BsonElement("establish_date")]
        public DateTime EstablishDate { get; set; }
    }
}