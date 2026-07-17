using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechDesk.API.DTOs.Ticket;
using TechDesk.API.Models;
using TechDesk.API.Services;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketDto dto)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ticket = await _ticketService.CreateAsync(dto, userId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = ticket.Id },
                ticket);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTicketDto dto)
        {
            bool updated = await _ticketService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _ticketService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignEngineer(int id, AssignEngineerDto dto)
        {
            bool assigned = await _ticketService.AssignEngineerAsync(id, dto);

            if (!assigned)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Engineer")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateTicketStatusDto dto)
        {
            bool updated = await _ticketService.UpdateStatusAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }


        [Authorize]
        [HttpGet("MyTickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var tickets = await _ticketService
                .GetMyTicketsAsync(userId);

            return Ok(tickets);
        }

        [Authorize(Roles = "Engineer")]
        [HttpGet("MyAssignedTickets")]
        public async Task<IActionResult> GetMyAssignedTickets()
        {
            var engineerId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var tickets = await _ticketService
                .GetMyAssignedTicketsAsync(engineerId);

            return Ok(tickets);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] TicketFilterDto dto)
        {
            var tickets = await _ticketService.SearchAsync(dto);

            return Ok(tickets);
        }
    }
}
