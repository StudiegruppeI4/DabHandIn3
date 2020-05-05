using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DabHandIn3.Models;
using DabHandIn3.Models.Objects;
using DabHandIn3.Services;

namespace DabHandIn3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly CircleService _circleService;

        public PostsController(PostService postservice, CircleService circleService)
        {
            _postService = postservice;
            _circleService = circleService;
        }

        [HttpGet]
        public ActionResult<List<Post>> Get()
        {
            return _postService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetPost")]
        public ActionResult<Post> Get(string id)
        {
            Post Post = _postService.Get(id);
            if (Post == null)
            {
                return NotFound();
            }
            return Post;

        }

        [HttpPost("{circleId}")]
        public ActionResult<Post> Create(string circleId, Post post)
        {
            Post result = _postService.CreatePost(post.Author.Id, post.Content, circleId);
            return CreatedAtRoute("GetPost", new { id = result.Id }, result);
        }

        [HttpPost("Comment/{postId}")]
        public ActionResult<Post> Create(string postId, Comment comment)
        {
            Post result = _postService.CreateComment(postId, comment);
            return CreatedAtRoute("GetPost", new { id = result.Id }, result);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Post newPost)
        {
            Post Post = _postService.Get(id);
            if (Post == null)
                return NotFound();

            _postService.Update(id, newPost);
            return Ok(newPost);
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            Post Post = _postService.Get(id);
            if (Post == null)
                return NotFound();
            _postService.Remove(Post.Id);

            return Ok(Post.Id);
        }
    }
}