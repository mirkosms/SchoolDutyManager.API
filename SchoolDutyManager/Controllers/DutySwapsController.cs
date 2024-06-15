using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;
using System.Collections.Generic;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DutySwapsController : ControllerBase
    {
        private readonly IDutySwapService _dutySwapService;

        public DutySwapsController(IDutySwapService dutySwapService)
        {
            _dutySwapService = dutySwapService;
        }

        [HttpGet]
        [Authorize(Roles = "Student,Teacher,Admin")]
        public ActionResult<List<DutySwap>> GetAllDutySwaps()
        {
            return Ok(_dutySwapService.GetAllDutySwaps());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Student,Teacher,Admin")]
        public ActionResult<DutySwap> GetDutySwapById(int id)
        {
            var dutySwap = _dutySwapService.GetDutySwapById(id);
            if (dutySwap == null)
            {
                return NotFound();
            }
            return Ok(dutySwap);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public ActionResult CreateDutySwap([FromBody] DutySwapRequestDto dutySwapRequestDto)
        {
            _dutySwapService.CreateDutySwap(dutySwapRequestDto);
            return CreatedAtAction(nameof(GetDutySwapById), new { id = dutySwapRequestDto.InitiatingStudentId }, dutySwapRequestDto);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Student,Teacher,Admin")]
        public ActionResult ApproveDutySwap(int id)
        {
            var userEmail = User.Identity.Name;
            var success = _dutySwapService.ApproveDutySwap(id, userEmail);
            if (!success)
            {
                return Forbid();
            }
            return NoContent();
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Student,Teacher,Admin")]
        public ActionResult RejectDutySwap(int id)
        {
            var userEmail = User.Identity.Name;
            var success = _dutySwapService.RejectDutySwap(id, userEmail);
            if (!success)
            {
                return Forbid();
            }
            return NoContent();
        }
    }
}
