using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using EntraTicket.Data;

namespace EntraTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TicketsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ComprarEntrada([FromBody] EntradaCompra entradaCompra)
        {
            try
            {
                string token = GenerarTokenUnico();

                DateTime activationTime = ObtenerHoraDelShow(entradaCompra.EventoID).AddHours(-1);

                string query = "INSERT INTO Entradas (UsuarioID, EventoID, LocalidadID, Token, ActivationTime) VALUES (@UsuarioID, @EventoID, @LocalidadID, @Token, @ActivationTime)";
                SqlParameter[] parameters = {
                    new SqlParameter("@UsuarioID", entradaCompra.UsuarioID),
                    new SqlParameter("@EventoID", entradaCompra.EventoID),
                    new SqlParameter("@LocalidadID", entradaCompra.LocalidadID),
                    new SqlParameter("@Token", token),
                    new SqlParameter("@ActivationTime", activationTime)
                };

                int result = _context.ExecuteNonQuery(query, parameters);

                if (result > 0)
                {
                    return Ok("Entrada comprada exitosamente. Tu token se activará una hora antes del show.");
                }
                return BadRequest("Hubo un problema al comprar la entrada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        private string GenerarTokenUnico()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[32];
                cryptoProvider.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer).Replace("=", "").Replace("+", "").Replace("/", "");
            }
        }

        private DateTime ObtenerHoraDelShow(int eventoID)
        {
            // Aquí iría la lógica para obtener la hora de inicio del evento según su ID.
            // Por ejemplo, una consulta a la base de datos que devuelva la hora del evento.
            return DateTime.Now.AddHours(2); // Ejemplo de retorno
        }

        [HttpGet("validar/{token}")]
        public IActionResult ValidarToken(string token)
        {
            try
            {
                string query = "SELECT ActivationTime FROM Entradas WHERE Token = @Token";
                SqlParameter[] parameters = {
                    new SqlParameter("@Token", token)
                };
                DataTable result = _context.ExecuteQuery(query, parameters);

                if (result.Rows.Count == 0)
                {
                    return NotFound("Token no encontrado.");
                }

                DateTime activationTime = Convert.ToDateTime(result.Rows[0]["ActivationTime"]);

                if (DateTime.Now >= activationTime && DateTime.Now <= activationTime.AddHours(1.5)) // 1.5 horas de ventana de tiempo
                {
                    return Ok("Token válido para ingresar.");
                }
                else
                {
                    return BadRequest("El token no es válido en este momento.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }

    public class EntradaCompra
    {
        public int UsuarioID { get; set; }
        public int EventoID { get; set; }
        public int LocalidadID { get; set; }
    }
}
