namespace EntraTicket.Models
{
    public class Event
    {
        public int Id { get; set; } // Asegúrate de que esto coincide con tu base de datos
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
    }
}
