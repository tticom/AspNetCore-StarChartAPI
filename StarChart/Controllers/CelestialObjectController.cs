using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name="GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject is null)
                return NotFound();

            var sats = _context.CelestialObjects.Where(co => co.OrbitedObjectId == id);
            celestialObject.Satellites.AddRange(sats);

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context.CelestialObjects.Where(co => co.Name == name).First();

            if (celestialObject is null)
                return NotFound();

            var sats = _context.CelestialObjects.Where(co => co.OrbitedObjectId == celestialObject.Id);
            celestialObject.Satellites.AddRange(sats);

            return Ok(celestialObject);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList<CelestialObject>();

            foreach(var co in celestialObjects)
            {
                if(!(co is null))
                {
                    var sats = _context.CelestialObjects.Where(c => c.OrbitedObjectId == co.Id);
                    co.Satellites.AddRange(sats);
                }
            }

            return Ok(celestialObjects);
        }
    }
}
