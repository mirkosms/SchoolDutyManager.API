using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            return StudentService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            var student = StudentService.Get(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            StudentService.Add(student);
            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            var existingStudent = StudentService.Get(id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            StudentService.Update(student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = StudentService.Get(id);
            if (student == null)
            {
                return NotFound();
            }

            StudentService.Delete(id);
            return NoContent();
        }
    }
}
