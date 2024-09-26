using EntraTicket.Repositories;
using Microsoft.AspNetCore.Mvc;
using EntraTicket.Models;
using System.Collections.Generic;

namespace Events.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetododepagoController : ControllerBase
    {
        private readonly Metodos _metodos;

        public MetododepagoController(Metodos metodos)
        {
            _metodos = metodos;
        }

        // GET api/metododepago
        [HttpGet]
        public ActionResult<IEnumerable<MetodoDePago>> Get()
        {
            var result = _metodos.ObtenerMetodosDePago();
            return Ok(result);
        }

        // GET api/metododepago/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public ActionResult<IEnumerable<MetodoDePago>> GetMetodosDePagoPorUsuario(int usuarioId)
        {
            var result = _metodos.ObtenerMetodosDePagoPorUsuario(usuarioId);
            if (result == null || result.Count == 0)
            {
                return NotFound($"No se encontraron métodos de pago para el usuario con ID {usuarioId}.");
            }
            return Ok(result);
        }

        // POST api/metododepago
        [HttpPost]
        public IActionResult Post([FromBody] MetodoDePago nuevoMetodo)
        {
            if (nuevoMetodo == null)
            {
                return BadRequest("El método de pago no puede ser nulo.");
            }

            _metodos.AgregarMetodoDePago(nuevoMetodo);
            return CreatedAtAction(nameof(Get), new { id = nuevoMetodo.MetodoID }, nuevoMetodo);
        }
    }
}
