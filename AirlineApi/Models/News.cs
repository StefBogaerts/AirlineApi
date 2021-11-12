using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AirlineApi.Models
{
    public class News
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("date_posted")]
        public DateTime Date_posted { get; set; }

        [BsonElement("article")]
        public string Article { get; set; }

        [BsonElement("image")]
        public string image { get; set; }
    }
}