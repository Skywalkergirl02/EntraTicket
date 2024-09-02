
using EntraTicket.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace EntraTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public EventosController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEventos()
        {
            string query = "SELECT * FROM Eventos";
            DataTable eventos = _context.ExecuteQuery(query);

            if (eventos.Rows.Count == 0)
            {
                return NotFound("No se encontraron eventos.");
            }

            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public IActionResult GetEventoById(int id)
        {
            string query = "SELECT * FROM Eventos WHERE EventoID = @id";
            SqlParameter[] parameters = {
                new SqlParameter("@id", id)
            };
            DataTable evento = _context.ExecuteQuery(query, parameters);

            if (evento.Rows.Count == 0)
            {
                return NotFound("Evento no encontrado.");
            }

            return Ok(evento);
        }

        [HttpPost]
        public IActionResult CrearEvento([FromBody] Evento evento)
        {
            string query = "INSERT INTO Eventos (Nombre, Fecha, Lugar, Descripcion) VALUES (@Nombre, @Fecha, @Lugar, @Descripcion)";
            SqlParameter[] parameters = {
                new SqlParameter("@Nombre", evento.Nombre),
                new SqlParameter("@Fecha", evento.Fecha),
                new SqlParameter("@Lugar", evento.Lugar),
                new SqlParameter("@Descripcion", evento.Descripcion ?? (object)DBNull.Value)
            };

            int result = _context.ExecuteNonQuery(query, parameters);

            if (result > 0)
            {
                return Ok("Evento creado exitosamente.");
            }
            return BadRequest("Hubo un problema al crear el evento.");
        }

        // Métodos PUT y DELETE para actualizar y eliminar eventos también pueden añadirse aquí.
    }

    public class Evento
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; }
        public string Descripcion { get; set; }
    }
}
