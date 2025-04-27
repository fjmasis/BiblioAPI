using BiblioAPI.Models;
using System.Data.SqlClient;
using System.Data;

namespace BiblioAPI.Services
{
    public class PrestamoService
    {
        private readonly string _connectionString;

        public PrestamoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");
        }

        public async Task<List<PrestamoModel>> ObtenerPrestamosAsync()
        {
            var prestamos = new List<PrestamoModel>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerPrestamos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            prestamos.Add(new PrestamoModel
                            {
                                Id = (int)reader["Id"],
                                IdUsuario = (int)reader["IdUsuario"],
                                IdLibro = (int)reader["IdLibro"],
                                FechaDevolucionEsperada = (DateTime)reader["FechaDevolucionEsperada"],
                                FechaDevolucionReal = reader["FechaDevolucionReal"] as DateTime?,
                                Estado = reader["Estado"].ToString()
                            });
                        }
                    }
                }
            }

            return prestamos;
        }
        public async Task<bool> ActualizarPrestamoAsync(int id, PrestamoModel prestamo)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarPrestamo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@FechaDevolucionReal", prestamo.FechaDevolucionReal);
                    cmd.Parameters.AddWithValue("@Estado", prestamo.Estado);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0; // true si se actualizó, false si no se encontró
                }
            }
        }

        public async Task RegistrarPrestamoAsync(PrestamoModel prestamo)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RegistrarPrestamo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", prestamo.IdUsuario);
                    cmd.Parameters.AddWithValue("@IdLibro", prestamo.IdLibro);
                    cmd.Parameters.AddWithValue("@FechaDevolucionEsperada", prestamo.FechaDevolucionEsperada);
                    cmd.Parameters.AddWithValue("@Estado", prestamo.Estado);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> EliminarPrestamoAsync(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarPrestamo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0; // true si se eliminó, false si no se encontró
                }
            }
        }
    }
}