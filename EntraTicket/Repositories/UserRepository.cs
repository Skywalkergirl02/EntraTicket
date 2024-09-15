using System.Threading.Tasks;
using EntraTicket.Models;

namespace EntraTicket.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Métodos para gestionar usuarios, como autenticación, registrooooo, etc.
    }
}
