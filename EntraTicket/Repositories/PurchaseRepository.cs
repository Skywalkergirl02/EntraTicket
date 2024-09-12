using System.Threading.Tasks;
using EntraTicket.Models;

namespace EntraTicket.Data
{
    public class PurchaseRepository
    {
        private readonly string _connectionString;

        public PurchaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Métodos para gestionar compras de entradas, historial, etc.
    }
}
