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
    public class DutiesController : ControllerBase
    {
        private readonly IDutyService _dutyService;

        public DutiesController(IDutyService dutyService)
        {
            _dutyService = dutyService;
        }

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

        [HttpGet("{id}")]
        public ActionResult<Duty> Get(int id)
        {
            var duty = _dutyService.GetDutyById(id);
            if (duty == null)
            {
                return NotFound();
            }
            return duty;
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Create(Duty duty)
        {
            _dutyService.AddDuty(duty);
            return CreatedAtAction(nameof(Get), new { id = duty.Id }, duty);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
