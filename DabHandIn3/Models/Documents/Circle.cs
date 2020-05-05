using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DabHandIn3.Models
{
    public class Circle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string CircleName { get; set; }
        public User Admin { get; set; }
        public bool Public { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> PostIds { get; set; }

    }
}
