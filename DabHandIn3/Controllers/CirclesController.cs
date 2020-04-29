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
    public class CirclesController : ControllerBase
    {
        private readonly CircleService _circleService;

        public CirclesController(CircleService Circleservice)
        {
            _circleService = Circleservice;
        }

        [HttpGet]
        public ActionResult<List<Circle>> Get()
        {
            return _circleService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetCircle")]
        public ActionResult<Circle> Get(string id)
        {
            Circle Circle = _circleService.Get(id);
            if (Circle == null)
            {
                return NotFound();
            }
            return Circle;

        }

        [HttpPost]
        public ActionResult<Circle> Create(Circle Circle)
        {
            _circleService.Create(Circle);
            return CreatedAtRoute("GetCircle", new { id = Circle.Id }, Circle);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Circle newCircle)
        {
            Circle Circle = _circleService.Get(id);
            if (Circle == null)
                return NotFound();

            _circleService.Update(id, newCircle);
            return Ok(newCircle);
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            Circle Circle = _circleService.Get(id);
            if (Circle == null)
                return NotFound();
            _circleService.Remove(Circle.Id);

            return Ok(Circle.Id);
        }
    }
}