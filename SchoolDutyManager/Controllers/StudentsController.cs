using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = StudentService.GetAll();
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return Ok(students);
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
                return BadRequest(new { message = "Id mismatch" });
            }

            var existingStudent = StudentService.Get(id);
            if (existingStudent == null)
            {
                return NotFound(new { message = "Student not found" });
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
