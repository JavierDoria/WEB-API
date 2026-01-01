using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VentasConcierto.Models;

namespace ConciertoVentas.Controllers
{
    [ApiController]
    [Route("api/sedes")]
    public class SedeController : ControllerBase
    {
        private readonly string cadena =
            "server=DESKTOP-QDE61J8;database=ConciertoVentas;User ID=sa;Password=123456;TrustServerCertificate=true;";

        // GET: api/sedes
        [HttpGet("Obtener")]
        public ActionResult<List<Sede>> ObtenerSedes()
        {
            List<Sede> listadoSede = new List<Sede>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand comando = new SqlCommand("SELECT * FROM Sede", cn);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Sede sede = new Sede
                    {
                        IdSede = reader.GetInt32(0),
                        NombreSede = reader.GetString(1),
                        Direccion = reader.GetString(2),
                        Correo = reader.GetString(3)
                    };

                    listadoSede.Add(sede);
                }
            }

            return Ok(listadoSede);
        }

        // POST: api/sedes
        [HttpPost("Guardar")]
        public ActionResult GuardarSede([FromBody] Sede sede)
        {
            int filasAfectadas;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand comando = new SqlCommand("INSERT INTO Sede (NombreSede, Direccion, Correo) VALUES (@NombreSede, @Direccion, @Correo)",cn);

                comando.Parameters.AddWithValue("@NombreSede", sede.NombreSede);
                comando.Parameters.AddWithValue("@Direccion", sede.Direccion);
                comando.Parameters.AddWithValue("@Correo", sede.Correo);

                filasAfectadas = comando.ExecuteNonQuery();
            }

            return Ok(filasAfectadas);
        }

        // DELETE: api/sedes/5
        [HttpDelete("Eliminar{id}")]
        public ActionResult EliminarSede(int id)
        {
            int filasAfectadas;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand comando = new SqlCommand(
                    "DELETE FROM Sede WHERE IdSede = @IdSede",
                    cn
                );

                comando.Parameters.AddWithValue("@IdSede", id);
                filasAfectadas = comando.ExecuteNonQuery();
            }

            return Ok(filasAfectadas);
        }

        // PUT: api/sedes/5
        [HttpPut("Actualizar{id}")]
        public ActionResult ActualizarSede(int id, [FromBody] Sede sede)
        {
            int filasAfectadas;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand comando = new SqlCommand(
                    "UPDATE Sede SET NombreSede = @NombreSede, Direccion = @Direccion, Correo = @Correo WHERE IdSede = @IdSede",
                    cn
                );

                comando.Parameters.AddWithValue("@IdSede", id);
                comando.Parameters.AddWithValue("@NombreSede", sede.NombreSede);
                comando.Parameters.AddWithValue("@Direccion", sede.Direccion);
                comando.Parameters.AddWithValue("@Correo", sede.Correo);

                filasAfectadas = comando.ExecuteNonQuery();
            }

            return Ok(filasAfectadas);
        }
    }
}