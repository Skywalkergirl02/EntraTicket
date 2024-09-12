using EntraTicket.Data;
using EntraTicket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EntraTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventRepository _eventRepository;

        public EventsController(EventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // POST api/events
        [HttpPost]
        public async Task<IActionResult> CrearEvento([FromBody] Event evento)
        {
            if (evento == null)
            {
                return BadRequest();
            }

            await _eventRepository.AddEventAsync(evento);

            return Ok();
        }
    }
}
