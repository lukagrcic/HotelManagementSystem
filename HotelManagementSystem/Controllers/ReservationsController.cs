using HotelManagementSystem.Application.Features.Reservations.Commands;
using HotelManagementSystem.Application.Features.Reservations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetReservationByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpGet("by-guest/{guestId}")]
        public async Task<IActionResult> GetByGuest(int guestId)
        {
            var result = await _mediator.Send(new GetReservationsByGuestQuery(guestId));

            return Ok(result);
        }

        [HttpGet("by-employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var result = await _mediator.Send(new GetReservationsByEmployeeQuery(employeeId));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllReservationsQuery());
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteReservationCommand(id));
            return success ? NoContent() : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReservationCommand command)
        {
            command.ReservationId = id;
            var success = await _mediator.Send(command);
            return success ? NoContent() : NotFound();
        }
    }
}
