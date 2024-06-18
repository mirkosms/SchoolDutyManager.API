using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;
using System.Linq;

namespace SchoolDutyManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DutiesController : ControllerBase
    {
        private readonly IDutyService _dutyService;

        public DutiesController(IDutyService dutyService)
        {
            _dutyService = dutyService;
        }

        [Authorize(Roles = "Student, Teacher, Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var duties = _dutyService.GetAllDuties();
            if (duties == null || !duties.Any())
            {
                return NotFound();
            }
            return Ok(duties);
        }

        [Authorize(Roles = "Student, Teacher, Admin")]
        [HttpGet("{id}")]
        public ActionResult<Duty> Get(int id)
        {
            var duty = _dutyService.GetDutyById(id);
            if (duty == null)
            {
                return NotFound();
            }
            return Ok(duty);
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public IActionResult Create(Duty duty)
        {
            _dutyService.AddDuty(duty);
            return CreatedAtAction(nameof(Get), new { id = duty.Id }, duty);
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Duty duty)
        {
            if (id != duty.Id)
            {
                return BadRequest();
            }

            var existingDuty = _dutyService.GetDutyById(id);
            if (existingDuty == null)
            {
                return NotFound();
            }

            _dutyService.UpdateDuty(duty);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var duty = _dutyService.GetDutyById(id);
            if (duty == null)
            {
                return NotFound();
            }

            _dutyService.DeleteDuty(id);
            return NoContent();
        }
    }
}
