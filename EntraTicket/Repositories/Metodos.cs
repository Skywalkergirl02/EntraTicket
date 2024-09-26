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

        // Obtener todos los métodos de pago
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

                    Console.WriteLine("Conexión abierta exitosamente."); // Verifica si la conexión se abrió

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No se encontraron métodos de pago en la base de datos.");
                        }

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

                    Console.WriteLine($"Total métodos de pago obtenidos: {metodos.Count}"); // Ver cuántos registros se obtienen
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

        // Obtener métodos de pago por usuario
        public List<MetodoDePago> ObtenerMetodosDePagoPorUsuario(int usuarioId)
        {
            var metodos = new List<MetodoDePago>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    const string query = "SELECT id, metodo_de_pago, descripcion FROM MetodosDePago WHERE UsuarioID = @UsuarioID"; // Asegúrate de que la tabla y columna existan
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UsuarioID", usuarioId);
                    connection.Open();

                    Console.WriteLine("Conexión abierta exitosamente para el usuario."); // Verifica si la conexión se abrió

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($"No se encontraron métodos de pago para el usuario con ID {usuarioId} en la base de datos.");
                        }
                        else
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

                    Console.WriteLine($"Total métodos de pago obtenidos para el usuario {usuarioId}: {metodos.Count}"); // Ver cuántos registros se obtienen
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

        // Agregar un nuevo método de pago
        public void AgregarMetodoDePago(MetodoDePago nuevoMetodo)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    const string query = "INSERT INTO MetodosDePago (metodo_de_pago, descripcion) VALUES (@MetodoDePago, @Descripcion)";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MetodoDePago", nuevoMetodo.NombreMetodo);
                    command.Parameters.AddWithValue("@Descripcion", nuevoMetodo.Descripcion);
                    connection.Open();

                    command.ExecuteNonQuery(); // Ejecuta el comando

                    Console.WriteLine("Método de pago agregado exitosamente.");
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
        }
    }
}
