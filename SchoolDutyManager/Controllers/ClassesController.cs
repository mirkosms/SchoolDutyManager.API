using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SchoolDutyManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly ILogger<ClassesController> _logger;
        private readonly IClassService _classService;

        public ClassesController(ILogger<ClassesController> logger, IClassService classService)
        {
            _logger = logger;
            _classService = classService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Attempting to get all classes");

            var userEmail = User.Identity.Name;
            _logger.LogInformation("User email: {0}", userEmail); // Logowanie adresu email użytkownika

            var classes = _classService.GetAll();
            if (classes == null || !classes.Any())
            {
                _logger.LogWarning("No classes found");
                return NotFound();
            }
            _logger.LogInformation("Classes retrieved successfully");
            return Ok(classes);
        }


        [HttpGet("{id}")]
        public ActionResult<Class> Get(int id)
        {
            _logger.LogInformation("Attempting to get class with id: {0}", id);
            var classObj = _classService.Get(id);
            if (classObj == null)
            {
                _logger.LogWarning("Class with id {0} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Class with id {0} retrieved successfully", id);
            return classObj;
        }

        [HttpPost]
        public IActionResult Create(Class classObj)
        {
            _logger.LogInformation("Attempting to create a new class");
            _classService.Add(classObj);
            _logger.LogInformation("Class created successfully with id {0}", classObj.Id);
            return CreatedAtAction(nameof(Get), new { id = classObj.Id }, classObj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Class classObj)
        {
            _logger.LogInformation("Attempting to update class with id {0}", id);
            if (id != classObj.Id)
            {
                _logger.LogWarning("Class id mismatch");
                return BadRequest();
            }

            var existingClass = _classService.Get(id);
            if (existingClass == null)
            {
                _logger.LogWarning("Class with id {0} not found", id);
                return NotFound();
            }

            _classService.Update(classObj);
            _logger.LogInformation("Class with id {0} updated successfully", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Attempting to delete class with id {0}", id);
            var classObj = _classService.Get(id);
            if (classObj == null)
            {
                _logger.LogWarning("Class with id {0} not found", id);
                return NotFound();
            }

            _classService.Delete(id);
            _logger.LogInformation("Class with id {0} deleted successfully", id);
            return NoContent();
        }
    }
}
