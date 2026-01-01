using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using VentasConcierto.Models;
using Microsoft.Data.SqlClient;
using System.Runtime;

namespace ConciertoVentas.Controllers
{
    [ApiController]
    [Route("Cliente")]
    public class ClienteController : ControllerBase
    {

        private readonly string cadena = "server=DESKTOP-QDE61J8;database=ConciertoVentas;User ID=sa;Password=123456;Integrated security=true;TrustServerCertificate=true;";


        [HttpGet("/Obtener")]
        public ActionResult<List<Cliente>> ListadoClientes()
        {
            List<Cliente> listadoBoleta = new List<Cliente>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand comando = new SqlCommand("select * from Cliente", cn);
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read()) 
            {
                Cliente cliente = new Cliente();
                cliente.IdCliente = reader.GetInt32(0);
                cliente.Nombre = reader.GetString(1);
                cliente.Apellido = reader.GetString(2);
                cliente.Dni = reader.GetString(3);
                cliente.Correo = reader.GetString(4);  
                listadoBoleta.Add(cliente);
            }
            cn.Close();
            return listadoBoleta;
        }

        [HttpPost("/Guardar")]
        public ActionResult GuardarCliente(Cliente cli)
        {
            int filasAfectadas=0;
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand comando = new SqlCommand("insert into  Cliente (nombre, apellido, dni, correo)  values (@nombre, @apellido, @dni, @correo) ", cn);
           
            comando.Parameters.AddWithValue("@nombre",cli.Nombre);
            comando.Parameters.AddWithValue("@apellido", cli.Apellido);
            comando.Parameters.AddWithValue("@dni", cli.Dni);
            comando.Parameters.AddWithValue("@correo",cli.Correo);
            filasAfectadas = comando.ExecuteNonQuery();
            cn.Close();
            return Ok( filasAfectadas);
        }


        [HttpDelete("/Eliminar")]
        public ActionResult EliminarCliente(int id)
        {
            int filasAfectadas = 0;
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand comando = new SqlCommand("delete Cliente where IdCliente = @IdCliente", cn);
            
            comando.Parameters.AddWithValue("@IdCliente", id);
            filasAfectadas = comando.ExecuteNonQuery();
            cn.Close();

            return Ok(filasAfectadas);
        }
        [HttpPut("/Actualizar")]
        public ActionResult ActualizarCliente(Cliente cliente)
        {
            int filasAfectadas = 0;
            List<Cliente> listadoBoleta = new List<Cliente>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand comando = new SqlCommand("update Cliente set Nombre = @Nombre, Apellido = @Apellido, Dni = @Dni, Correo = @Correo where IdCliente = @IdCliente", cn);

            comando.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
            comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
            comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
            comando.Parameters.AddWithValue("@dni", cliente.Dni);
            comando.Parameters.AddWithValue("@correo", cliente.Correo);
            filasAfectadas = comando.ExecuteNonQuery();
            cn.Close();
            return Ok(filasAfectadas);
        }
    }
}
