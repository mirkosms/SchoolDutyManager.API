using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeachersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Teacher>> GetAll()
        {
            return TeacherService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Teacher> Get(int id)
        {
            var teacher = TeacherService.Get(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return teacher;
        }

        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {
            TeacherService.Add(teacher);
            return CreatedAtAction(nameof(Get), new { id = teacher.Id }, teacher);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            var existingTeacher = TeacherService.Get(id);
            if (existingTeacher == null)
            {
                return NotFound();
            }

            TeacherService.Update(teacher);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var teacher = TeacherService.Get(id);
            if (teacher == null)
            {
                return NotFound();
            }

            TeacherService.Delete(id);
            return NoContent();
        }
    }
}
