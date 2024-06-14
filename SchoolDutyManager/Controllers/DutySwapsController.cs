using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DutySwapsController : ControllerBase
    {
        private readonly IDutySwapService _dutySwapService;
        private readonly IUserService _userService;

        public DutySwapsController(IDutySwapService dutySwapService, IUserService userService)
        {
            _dutySwapService = dutySwapService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult RequestDutySwap(DutySwap dutySwap)
        {
            dutySwap.Status = "Pending";
            _dutySwapService.Add(dutySwap);
            return CreatedAtAction(nameof(GetDutySwap), new { id = dutySwap.Id }, dutySwap);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Student,Teacher,Admin")]
        public IActionResult GetDutySwap(int id)
        {
            var dutySwap = _dutySwapService.Get(id);
            if (dutySwap == null)
            {
                return NotFound();
            }
            return Ok(dutySwap);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult GetAllDutySwaps()
        {
            var dutySwaps = _dutySwapService.GetAll();
            return Ok(dutySwaps);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult ApproveDutySwap(int id)
        {
            var dutySwap = _dutySwapService.Get(id);
            if (dutySwap == null)
            {
                return NotFound();
            }

            dutySwap.Status = "Approved";
            _dutySwapService.Update(dutySwap);
            return NoContent();
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult RejectDutySwap(int id)
        {
            var dutySwap = _dutySwapService.Get(id);
            if (dutySwap == null)
            {
                return NotFound();
            }

            dutySwap.Status = "Rejected";
            _dutySwapService.Update(dutySwap);
            return NoContent();
        }
    }
}
