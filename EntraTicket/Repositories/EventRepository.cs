using System.Data.SqlClient;
using System.Threading.Tasks;
using EntraTicket.Models;

namespace EntraTicket.Data
{
    public class EventRepository
    {
        private readonly string _connectionString;

        public EventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddEventAsync(Event evt)
        {
            var query = "INSERT INTO eventos (name, description, date, location, price) VALUES (@Name, @Description, @Date, @Location, @Price)";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", evt.Name);
                    command.Parameters.AddWithValue("@Description", evt.Description);
                    command.Parameters.AddWithValue("@Date", evt.Date);
                    command.Parameters.AddWithValue("@Location", evt.Location);
                    command.Parameters.AddWithValue("@Price", evt.Price);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
