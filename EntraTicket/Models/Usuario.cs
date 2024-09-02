namespace EntraTicket.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
