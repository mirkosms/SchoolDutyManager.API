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
        [HttpGet]
        public IActionResult GetAll()
        {
            var duties = DutyService.GetAll();
            if (duties == null || !duties.Any())
            {
                return NotFound();
            }
            return Ok(duties);
        }

        [HttpGet("{id}")]
        public ActionResult<Duty> Get(int id)
        {
            var duty = DutyService.Get(id);
            if (duty == null)
            {
                return NotFound();
            }
            return duty;
        }

        [HttpPost]
        public IActionResult Create(Duty duty)
        {
            DutyService.Add(duty);
            return CreatedAtAction(nameof(Get), new { id = duty.Id }, duty);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Duty duty)
        {
            if (id != duty.Id)
            {
                return BadRequest();
            }

            var existingDuty = DutyService.Get(id);
            if (existingDuty == null)
            {
                return NotFound();
            }

            DutyService.Update(duty);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var duty = DutyService.Get(id);
            if (duty == null)
            {
                return NotFound();
            }

            DutyService.Delete(id);
            return NoContent();
        }
    }
}
