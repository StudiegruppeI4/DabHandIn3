using System;
using DabHandIn3.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DabHandIn3.Models.Objects;
using Microsoft.EntityFrameworkCore.Query;

namespace DabHandIn3.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;
        private readonly CircleService _circleService;
        private readonly UserService _userService;

        public PostService(IDabHandIn3DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _posts = db.GetCollection<Post>(settings.PostsCollectionName);
            _circleService = new CircleService(settings);
            _userService = new UserService(settings);
        }

        public List<Post> Get()
        {
            return _posts.Find<Post>(post => true).ToList();
        }

        public Post Get(string id)
        {
            return _posts.Find<Post>(post => post.Id == id).FirstOrDefault();
        }

        public List<Post> ShowUserFeed(string userId)
        {
            List<Circle> userCircles = _circleService.GetCirclesFromUserId(userId);
            return FilterOutBlockedUserPosts(GetPostsFromCircles(userCircles), userId);
        }

        public List<Post> ShowUserWall(string userId, string guestId)
        {
            List<Circle> userCircles = _circleService.GetCirclesFromUserId(userId);
            userCircles = userCircles.Where(c => c.UserIds.Contains(guestId) || c.Public).ToList();
            return FilterOutBlockedUserPosts(GetPostsFromCircles(userCircles), userId);
        }

        public Post CreatePost(string userId, Content content, string circleId)
        {
            Post post = new Post();
            post.Author = _userService.Get(userId);
            post.Content = content;
            post.CreationTime = DateTime.Now;
            post.Comments = new List<Comment>();

            Create(post, circleId);

            return post;
        }

        public Post CreateComment(string postId, Comment comment)
        {
            Post post = Get(postId);
            comment.CreationTime = DateTime.Now;
            post.Comments.Add(comment);
            Update(post.Id, post);
            return post;
        }

        private List<Post> FilterOutBlockedUserPosts(List<Post> posts, string userId)
        {
            User user = _userService.Get(userId);
            foreach (var post in posts)
            {
                if (user.BlockedUsers.Contains(post.Author.Id) || post.Author.BlockedUsers.Contains(userId))
                {
                    posts.Remove(post);
                }
            }

            return posts;
        }

        private List<Post> GetPostsFromCircles(List<Circle> circles)
        {
            List<Post> posts = new List<Post>();
            foreach (var circle in circles)
            {
                posts.AddRange(GetPostsInCircle(circle));
            }

            return posts;
        }

        private List<Post> GetPostsInCircle(Circle circle)
        {
            return _posts.Find<Post>(post => circle.PostIds.Contains(post.Id)).ToList();
        }

        public Post Create(Post post, string circleId)
        {
            _posts.InsertOne(post);
            Circle circle = _circleService.Get(circleId);
            circle.PostIds.Add(post.Id);
            
            _circleService.Update(circle.Id, circle);
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
