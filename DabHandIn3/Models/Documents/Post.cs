using DabHandIn3.Models.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DabHandIn3.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public User Author { get; set; }
        public Content Content { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
