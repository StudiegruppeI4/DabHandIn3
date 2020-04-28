using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace DabHandIn3.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string UserName { get; set; }
        public string Gender { get; set; }
        public List<Circle> Circles { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

    }
}
