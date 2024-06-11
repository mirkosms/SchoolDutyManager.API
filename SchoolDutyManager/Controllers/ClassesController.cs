using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Class>> GetAll()
        {
            return ClassService.GetAll();
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
