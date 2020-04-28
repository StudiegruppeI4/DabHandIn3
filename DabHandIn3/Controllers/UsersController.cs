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

        public UsersController(UserService userService)
        {
            _userService = userService;
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