using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DabHandIn3.Models;
using DabHandIn3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DabHandIn3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewsController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly CircleService _circleService;
        private readonly UserService _userService;

        public ViewsController(PostService postService, CircleService circleService, UserService userService)
        {
            _postService = postService;
            _circleService = circleService;
            _userService = userService;
        }

        // POST: api/Views
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Views/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
