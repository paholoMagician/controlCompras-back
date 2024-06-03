using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using seinAppBackend.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace seinAppBackend.Controllers
{
    [Route("api/Producto")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        readonly private sheinAppContext _context;

        public ProductoController(sheinAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GuardarProducto")]
        public async Task<IActionResult> GuardarProducto([FromBody] Producto model)
        {

            if (ModelState.IsValid)
            {
                _context.Producto.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(model);
                }
                else
                {
                    return BadRequest("Datos incorrectos");
                }
            }
            else
            {
                return BadRequest("ERROR");
            }

        }

        [HttpGet("ObtenerProductosCliente/{iduser}/{idcliente}/{estado}")]
        public async Task<IActionResult> ObtenerProductosCliente([FromRoute] int iduser, [FromRoute] int idcliente, [FromRoute] int estado)
        {
            // Obtener la fecha actual sin la parte de la hora
            DateTime today = DateTime.Today;

            // Usar LINQ para consultar la base de datos
            var productos = await _context.Producto
                                          .Where(p => p.Idusuario == iduser
                                                  && p.Idcliente == idcliente
                                                  && p.Estado == estado)
                                          .Select(p => new
                                          {
                                              p.Id,
                                              p.Idusuario,
                                              p.Idcliente,
                                              p.Tienda,
                                              p.NombreProducto,
                                              p.Precioacliente,
                                              p.Preciodeapp,
                                              p.Urlimagen,
                                              p.Fecrea,
                                              p.Estado
                                          })
                                          .ToListAsync();

            if (productos == null || productos.Count == 0)
            {
                return NotFound("No se encontraron productos para el cliente y usuario especificados en la fecha actual.");
            }

            return Ok(productos);
        }

        [HttpGet("EliminarProductosCliente/{idproducto}")]
        public async Task<IActionResult> EliminarProductosCliente([FromRoute] int idproducto)
        {

            string Sentencia = " DELETE FROM producto where id = @idprod ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idprod", idproducto));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

        [HttpGet("obtenerHistorialProductos/{idusuario}/{idcliente}")]
        public async Task<IActionResult> obtenerHistorialProductos([FromRoute] int idusuario, [FromRoute] int idcliente )
        {

            string Sentencia = " exec HistorialCompras @idUser, @idcli ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idUser", idusuario));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idcli", idcliente));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

        [HttpGet("ActualizarProductosClienteEstado/{st}/{idUser}/{idCliente}")]
        public async Task<IActionResult> ActualizarProductosClienteEstado([FromRoute] int st, [FromRoute] int idUser, [FromRoute] int idCliente )
        {

            string Sentencia = "update producto set estado = @estado where idusuario = @idUsuario and idcliente = @idCli and estado = 0";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@estado",    st));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idUsuario", idUser));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idCli",     idCliente));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

    }
}
