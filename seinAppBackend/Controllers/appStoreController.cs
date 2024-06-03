using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using seinAppBackend.Models;
using System.Data;

namespace seinAppBackend.Controllers
{
    [Route("api/AppStore")]
    [ApiController]
    public class appStoreController : ControllerBase
    {

        readonly private sheinAppContext _context;

        public appStoreController(sheinAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("guardarAppStore")]
        public async Task<IActionResult> guardarAppStore( [FromBody] DataAppStore model )
        {

            if (ModelState.IsValid)
            {
                _context.DataAppStore.Add(model);
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

        [HttpGet("obtenerAppStore/{usercrea}")]
        public async Task<IActionResult> ObtenerAppStore([FromRoute] int usercrea)
        {
            // Usando el contexto de la base de datos para consultar la tabla
            var results = await _context.DataAppStore
                                        .Where(d => d.Usercrea == usercrea && d.Estado == 1)
                                        .ToListAsync();

            if (results == null || !results.Any())
            {
                return NotFound("No se ha podido obtener...");
            }

            return Ok(results);
        }

        [HttpPut]
        [Route("actualizarAppStore/{id}")]
        public async Task<IActionResult> actualizarAppStore([FromRoute] int id, [FromBody] DataAppStore model)
        {
            if (id != model.Id)
            {
                return BadRequest("No existe la asignacion");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }

        [HttpGet("eliminarAppStore/{id}")]
        public async Task<IActionResult> eliminarAppStore([FromRoute] int id )
        {
            string Sentencia = " update dataAppStore set estado = 2 where id = @idstore ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idstore", id));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido obtener...");
            }

            return Ok(dt);


        }

    }
}
