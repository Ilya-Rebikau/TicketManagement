using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.Tickets;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Controllers
{
    [Authorize(Roles = "admin, event manager")]
    [Route("[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        /// <summary>
        /// TicketService object.
        /// </summary>
        private readonly IService<TicketDto> _service;

        /// <summary>
        /// IConverter object.
        /// </summary>
        private readonly IConverter<TicketDto, TicketModel> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsController"/> class.
        /// </summary>
        /// <param name="service">TicketService object.</param>
        /// <param name="converter">IConverter object.</param>
        public TicketsController(IService<TicketDto> service, IConverter<TicketDto, TicketModel> converter)
        {
            _service = service;
            _converter = converter;
        }

        /// <summary>
        /// Get all tickets.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("gettickets")]
        public async Task<IActionResult> GetTickets([FromQuery] int pageNumber)
        {
            var tickets = await _service.GetAllAsync(pageNumber);
            return Ok(await _converter.ConvertSourceModelRangeToDestinationModelRange(tickets));
        }

        /// <summary>
        /// Details about ticket.
        /// </summary>
        /// <param name="id">Id of ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var ticket = await _service.GetByIdAsync(id);
            return ticket is null ? NotFound() : Ok(ticket);
        }

        /// <summary>
        /// Create ticket.
        /// </summary>
        /// <param name="ticketModel">Adding ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TicketModel ticketModel)
        {
            await _service.CreateAsync(await _converter.ConvertDestinationToSource(ticketModel));
            return Ok();
        }

        /// <summary>
        /// Edit ticket.
        /// </summary>
        /// <param name="id">Id of editing ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var updatingTicket = await _service.GetByIdAsync(id);
            return updatingTicket is null ? NotFound() : Ok(updatingTicket);
        }

        /// <summary>
        /// Edit ticket.
        /// </summary>
        /// <param name="id">Id of editing ticket.</param>
        /// <param name="ticket">Edited ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] TicketModel ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(await _converter.ConvertDestinationToSource(ticket));
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TicketExists(ticket.Id))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict();
                }
            }
        }

        /// <summary>
        /// Delete ticket.
        /// </summary>
        /// <param name="id">Id of deleting ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletingTicket = await _service.GetByIdAsync(id);
            return deletingTicket is null ? NotFound() : Ok(deletingTicket);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Check that ticket exists.
        /// </summary>
        /// <param name="id">Id of ticket.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> TicketExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
