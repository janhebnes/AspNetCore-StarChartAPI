using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context) => _context = context;

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var model = _context.CelestialObjects.Find(id);
            if (model == null)
                return NotFound();

            model.Satellites = _context.CelestialObjects.Where(d => d.OrbitedObjectId == model.Id).ToList();
            return Ok(model);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var models = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (models.Count == 0)
                return NotFound();

            foreach (var model in models)
            {
                model.Satellites = _context.CelestialObjects.Where(d => d.OrbitedObjectId == model.Id).ToList();
            }

            return Ok(models);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var models = _context.CelestialObjects.ToList();
            foreach (var model in models)
            {
                model.Satellites = _context.CelestialObjects.Where(d => d.OrbitedObjectId == model.Id).ToList();
            }
            return Ok(models);
        }

        [HttpPost(Name = "Create")]
        public IActionResult Create([FromBody] CelestialObject model)
        {
            _context.CelestialObjects.Add(model);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = model.Id}, model);
        }

        [HttpPut("{id}",Name = "Update")]
        public IActionResult Update(int id, [FromBody] CelestialObject updatedModel)
        {
            var model = _context.CelestialObjects.Find(id);
            if (model == null)
                return NotFound();

            model.Name = updatedModel.Name;
            model.OrbitalPeriod = updatedModel.OrbitalPeriod;
            model.OrbitedObjectId = updatedModel.OrbitedObjectId;

            _context.CelestialObjects.Update(model);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var model = _context.CelestialObjects.Find(id);
            if (model == null)
                return NotFound();

            model.Name = name;

            _context.CelestialObjects.Update(model);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = _context.CelestialObjects.Find(id);
            if (model == null)
                return NotFound();

            var remove = _context.CelestialObjects.Where(c => c.OrbitedObjectId == model.Id).ToList();
            remove.Add(model);
            _context.CelestialObjects.RemoveRange(remove);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
