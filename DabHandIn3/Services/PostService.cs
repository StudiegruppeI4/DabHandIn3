using DabHandIn3.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DabHandIn3.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService(IDabHandIn3DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _posts = db.GetCollection<Post>(settings.PostsCollectionName);
        }

        public List<Post> Get()
        {
            return _posts.Find<Post>(post => true).ToList();
        }

        public Post Get(string id)
        {
            return _posts.Find<Post>(post => post.Id == id).FirstOrDefault();
        }

        public Post Create(Post post)
        {
            _posts.InsertOne(post);
            return post;
        }

        public void Update(string id, Post postIn)
        {
            _posts.ReplaceOne(post => post.Id == id, postIn);
        }

        public void Remove(string id)
        {
            _posts.DeleteOne(post => post.Id == id);
        }
        public void Remove(Post postIn)
        {
            _posts.DeleteOne(post => post.Id == postIn.Id);
        }
    }
}
