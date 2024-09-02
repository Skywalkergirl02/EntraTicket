using EntraTicket.Data;
using EntraTicket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EntraTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("registro")]
        public IActionResult Registro([FromBody] Usuario usuario)
        {
            // Aquí se debería encriptar la contraseña
            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

            string query = "INSERT INTO Usuarios (Nombre, Correo, Contraseña) VALUES (@Nombre, @Correo, @Contraseña)";
            SqlParameter[] parameters = {
                new SqlParameter("@Nombre", usuario.Nombre),
                new SqlParameter("@Correo", usuario.Correo),
                new SqlParameter("@Contraseña", usuario.Contraseña)
            };

            int result = _context.ExecuteNonQuery(query, parameters);

            if (result > 0)
            {
                return Ok("Usuario registrado exitosamente.");
            }
            return BadRequest("Hubo un problema al registrar al usuario.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuario usuario)
        {
            string query = "SELECT * FROM Usuarios WHERE Correo = @Correo";
            SqlParameter[] parameters = {
                new SqlParameter("@Correo", usuario.Correo)
            };

            DataTable result = _context.ExecuteQuery(query, parameters);
            if (result.Rows.Count == 0)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            var usuarioDB = result.Rows[0];
            bool validPassword = BCrypt.Net.BCrypt.Verify(usuario.Contraseña, usuarioDB["Contraseña"].ToString());

            if (!validPassword)
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            var token = GenerateJwtToken(usuario.Correo);

            return Ok(new { token });
        }

        private string GenerateJwtToken(string correo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, correo),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
