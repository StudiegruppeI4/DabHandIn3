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
            return _posts.Find<Post>(Post => true).ToList();
        }

        public Post Get(string id)
        {
            return _posts.Find<Post>(Post => Post.Id == id).FirstOrDefault();
        }

        public Post Create(Post Post)
        {
            _posts.InsertOne(Post);
            return Post;
        }

        public void Update(string id, Post Post)
        {
            _posts.ReplaceOne(Post => Post.Id == id, Post);
        }

        public void Remove(string id)
        {
            _posts.DeleteOne(Post => Post.Id == id);
        }
        public void Remove(Post Post)
        {
            _posts.DeleteOne(Post => Post.Id == Post.Id);
        }
    }
}
