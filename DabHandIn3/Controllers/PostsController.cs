using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DabHandIn3.Models;
using DabHandIn3.Services;

namespace DabHandIn3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService Postservice)
        {
            _postService = Postservice;
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

        [HttpPost]
        public ActionResult<Post> Create(Post Post)
        {
            _postService.Create(Post);
            return CreatedAtRoute("GetPost", new { id = Post.Id }, Post);
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