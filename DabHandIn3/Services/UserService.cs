using DabHandIn3.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DabHandIn3.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IDabHandIn3DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _users = db.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get()
        {
            return _users.Find<User>(User => true).ToList();
        }

        public User Get(string id)
        {
            return _users.Find<User>(User => User.Id == id).FirstOrDefault();
        }

        public User Create(User User)
        {
            _users.InsertOne(User);
            return User;
        }

        public void Update(string id, User User)
        {
            _users.ReplaceOne(User => User.Id == id, User);
        }

        public void Remove(string id)
        {
            _users.DeleteOne(User => User.Id == id);
        }
        public void Remove(User User)
        {
            _users.DeleteOne(User => User.Id == User.Id);
        }
    }
}
