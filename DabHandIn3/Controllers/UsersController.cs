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
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PostService _postService;

        public UsersController(UserService userService, PostService postService)
        {
            _userService = userService;
            _postService = postService;
        }
    
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return _userService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            User user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("Feed/{id}")]
        public ActionResult<List<Post>> GetFeed(string id)
        {
            return _postService.ShowUserFeed(id).OrderByDescending(p => p.CreationTime).Take(10).ToList();
        }

        [HttpGet("Wall/{userId}-{guestId}")]
        public ActionResult<List<Post>> GetWall(string userId, string guestId)
        {
            return _postService.ShowUserWall(userId, guestId).OrderByDescending(p => p.CreationTime).Take(10).ToList();
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            _userService.Create(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User newUser)
        {
            User user = _userService.Get(id);
            if (user == null)
                return NotFound();

            _userService.Update(id, newUser);
            return Ok(newUser);       
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            User user = _userService.Get(id);
            if (user == null)
                return NotFound();
            _userService.Remove(user.Id);

            return Ok(user.Id);
        }
    }
}