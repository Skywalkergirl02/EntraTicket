namespace EntraTicket.Models
{
    public class MetodoDePago
    {
        public int MetodoID { get; set; }  // Se mapea al campo 'id'
        public string NombreMetodo { get; set; }  // Se mapea al campo 'metodo_de_pago'
        public string Descripcion { get; set; }  // Se mapea al campo 'descripcion'
    }
}