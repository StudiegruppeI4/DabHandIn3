using DabHandIn3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DabHandIn3.Services
{

    public class CircleService
    {
        private readonly IMongoCollection<Circle> _circles;

        public CircleService(IDabHandIn3DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _circles = db.GetCollection<Circle>(settings.CirclesCollectionName);
        }

        public List<Circle> Get()
        {
            return _circles.Find<Circle>(circle => true).ToList();
        }

        public Circle Get(string id)
        {
            return _circles.Find<Circle>(circle => circle.Id == id).FirstOrDefault();
        }

        public List<Circle> GetCirclesFromUserId(string userId)
        {
            return _circles.Find<Circle>(circle => circle.UserIds.Contains(userId)).ToList();
        }

        public Circle Create(Circle circle)
        {
            _circles.InsertOne(circle);
            return circle;
        }

        public void Update(string id, Circle circleIn)
        {
            _circles.ReplaceOne(circle => circle.Id == id, circleIn);
        }

        public void Remove(string id)
        {
            _circles.DeleteOne(circle => circle.Id == id);
        }
        public void Remove(Circle circleIn)
        {
            _circles.DeleteOne(circle => circle.Id == circleIn.Id);
        }
    }
}
