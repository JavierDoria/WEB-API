using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using VentasConcierto.Models;

namespace ConciertoVentas.Controllers
{
    [ApiController]
    [Route("api/boleta")]
    public class BoletaController : ControllerBase
    {
        private readonly string cadena = "server=DESKTOP-QDE61J8;database=ConciertoVentas;User ID=sa;Password=123456;TrustServerCertificate=true;";
        [HttpGet]
        public ActionResult<List<Boleta>> ObtenerBoleta()
        {
            List<Boleta> listadoBoleta = new List<Boleta>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand comando = new SqlCommand("SELECT b.IdBoleta,c.Dni AS DniCliente,s.NombreSede AS NombreSede, b.Empresa,b.Lugar, b.Evento, b.Costo FROM Boleta b INNER JOIN Cliente c ON b.IdCliente = c.IdCliente INNER JOIN Sede s ON b.IdSede = s.IdSede", cn);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    listadoBoleta.Add(new Boleta
                    {
                        IdBoleta = reader.GetInt32(0),
                        DniCliente = reader.GetString(1),
                        NombreSede = reader.GetString(2),
                        Empresa = reader.GetString(3),
                        Lugar = reader.GetString(4),
                        Evento = reader.GetString(5),
                        Costo = reader.GetDecimal(6)
                    });
                }
            }
            return Ok(listadoBoleta);
        }
        [HttpPost]
        public ActionResult GuardarBoleta([FromBody] Boleta request)
        {
            int filasAfectadas;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand comando = new SqlCommand(@"
            INSERT INTO Boleta (IdCliente, IdSede, Empresa, Lugar, Evento, Costo)
            SELECT  
                c.IdCliente,
                s.IdSede,
                @Empresa,
                @Lugar,
                @Evento,
                @Costo
            FROM Cliente c
            INNER JOIN Sede s 
                ON s.NombreSede = @NombreSede
            WHERE c.Dni = @DniCliente
                ", cn);

                comando.Parameters.AddWithValue("@DniCliente", request.DniCliente.Trim());
                comando.Parameters.AddWithValue("@NombreSede", request.NombreSede.Trim());
                comando.Parameters.AddWithValue("@Empresa", request.Empresa.Trim());
                comando.Parameters.AddWithValue("@Lugar", request.Lugar);
                comando.Parameters.AddWithValue("@Evento", request.Evento);
                comando.Parameters.AddWithValue("@Costo", request.Costo);

                filasAfectadas = comando.ExecuteNonQuery();
                if (filasAfectadas == 0)
                {
                    return BadRequest("No se insertó la boleta. El DNI del cliente o la sede no existen.");
                }
            }

            return Ok("Boleta registrada correctamente");

        }
        [HttpDelete("{id}")]
        public ActionResult EliminarBoleta(int id)
        {
            int filasAfectadas;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand comando = new SqlCommand(
                    "DELETE FROM Boleta WHERE IdBoleta = @IdBoleta",
                    cn
                );

                comando.Parameters.AddWithValue("@IdBoleta", id);
                filasAfectadas = comando.ExecuteNonQuery();
            }

            return Ok(filasAfectadas);
        }
        [HttpPut("{id}")]
        public ActionResult ActualizarBoleta(int id, [FromBody] Boleta boleta)
        {
            int filasAfectadas;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand comando = new SqlCommand(
                    "UPDATE Boleta SET Empresa = @Empresa, Lugar = @Lugar, Evento = @Evento, Costo = @Costo WHERE IdBoleta = @IdBoleta",
                    cn
                );

                comando.Parameters.AddWithValue("@IdBoleta", id);
                comando.Parameters.AddWithValue("@Empresa", boleta.NombreSede);
                comando.Parameters.AddWithValue("@Lugar", boleta.Lugar);
                comando.Parameters.AddWithValue("@Evento", boleta.Evento);
                comando.Parameters.AddWithValue("@Costo", boleta.Costo);

                filasAfectadas = comando.ExecuteNonQuery();
            }

            return Ok(filasAfectadas);
        }
    } 
}
