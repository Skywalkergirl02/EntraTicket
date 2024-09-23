using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EntraTicket.Models;
using Microsoft.Extensions.Configuration;

namespace EntraTicket.Repositories

{
    public class Metodos
    {
        private readonly string _connectionString;

        public Metodos(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new ArgumentNullException(nameof(configuration), "La cadena de conexión no puede ser nula");
        }


        public List<MetodoDePago> ObtenerMetodosDePago()
        {
            var metodos = new List<MetodoDePago>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    const string query = "SELECT id, metodo_de_pago, descripcion FROM MetodosDePago";
                    var command = new SqlCommand(query, connection);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var metodo = new MetodoDePago
                            {
                                MetodoID = reader.GetInt32(reader.GetOrdinal("id")),
                                NombreMetodo = reader.GetString(reader.GetOrdinal("metodo_de_pago")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion"))
                            };
                            metodos.Add(metodo);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de SQL: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return metodos;
        }

    }
}
