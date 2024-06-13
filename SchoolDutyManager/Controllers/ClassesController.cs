using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;
using System.Linq;

namespace SchoolDutyManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var classes = ClassService.GetAll();
            if (classes == null || !classes.Any())
            {
                return NotFound();
            }
            return Ok(classes);
        }

        [HttpGet("{id}")]
        public ActionResult<Class> Get(int id)
        {
            var classObj = ClassService.Get(id);
            if (classObj == null)
            {
                return NotFound();
            }
            return classObj;
        }

        [HttpPost]
        public IActionResult Create(Class classObj)
        {
            ClassService.Add(classObj);
            return CreatedAtAction(nameof(Get), new { id = classObj.Id }, classObj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Class classObj)
        {
            if (id != classObj.Id)
            {
                return BadRequest();
            }

            var existingClass = ClassService.Get(id);
            if (existingClass == null)
            {
                return NotFound();
            }

            ClassService.Update(classObj);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var classObj = ClassService.Get(id);
            if (classObj == null)
            {
                return NotFound();
            }

            ClassService.Delete(id);
            return NoContent();
        }
    }
}
