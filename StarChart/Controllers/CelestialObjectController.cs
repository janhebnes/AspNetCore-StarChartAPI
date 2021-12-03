using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
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
    }
}
