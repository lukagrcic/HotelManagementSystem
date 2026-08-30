using HotelManagementSystem.Application.Features.Guests.Commands;
using HotelManagementSystem.Application.Features.Guests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GuestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGuestCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet("by-jmbg/{jmbg}")]
        public async Task<IActionResult> GetByJmbg(string jmbg)
        {
            var result = await _mediator.Send(new GetGuestByJmbgQuery(jmbg));
            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetGuestByIdQuery(id));
            if(result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllGuestsQuery());
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateGuestCommand command)
        {
            command.GuestId = id;
            var success = await _mediator.Send(command);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteGuestCommand(id));
            return success ? NoContent() : NotFound();
        }
       
    }
}
