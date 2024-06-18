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
    public class StudentsController : ControllerBase
    {
        [Authorize(Roles = "Student, Teacher, Admin")]
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

        [Authorize(Roles = "Student, Teacher, Admin")]
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

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public IActionResult Create(Student student)
        {
            StudentService.Add(student);
            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        [Authorize(Roles = "Teacher, Admin")]
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

        [Authorize(Roles = "Teacher, Admin")]
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
