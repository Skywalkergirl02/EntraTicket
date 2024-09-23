using EntraTicket.Repositories;
using Microsoft.AspNetCore.Mvc;
using EntraTicket.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

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


        // POST api/metododepago
        [HttpPost]
        public IActionResult Post([FromBody] MetodoDePago nuevoMetodo)
        {
            if (nuevoMetodo == null)
            {
                return BadRequest("El método de pago no puede ser nulo.");
            }

            return CreatedAtAction(nameof(Get), new { id = nuevoMetodo.MetodoID }, nuevoMetodo);
        }
    }
}

