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
    [Route("[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly ILogger<ClassesController> _logger;

        public ClassesController(ILogger<ClassesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Attempting to get all classes");
            var classes = ClassService.GetAll();
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
            var classObj = ClassService.Get(id);
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
            ClassService.Add(classObj);
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

            var existingClass = ClassService.Get(id);
            if (existingClass == null)
            {
                _logger.LogWarning("Class with id {0} not found", id);
                return NotFound();
            }

            ClassService.Update(classObj);
            _logger.LogInformation("Class with id {0} updated successfully", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Attempting to delete class with id {0}", id);
            var classObj = ClassService.Get(id);
            if (classObj == null)
            {
                _logger.LogWarning("Class with id {0} not found", id);
                return NotFound();
            }

            ClassService.Delete(id);
            _logger.LogInformation("Class with id {0} deleted successfully", id);
            return NoContent();
        }
    }
}
