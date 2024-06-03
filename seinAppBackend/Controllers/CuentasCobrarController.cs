using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using seinAppBackend.Models;
using System.Data;

namespace seinAppBackend.Controllers
{
    [Route("api/cuentasPorCobrar")]
    [ApiController]
    public class CuentasCobrarController : ControllerBase
    {

        readonly private sheinAppContext _context;

        public CuentasCobrarController(sheinAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GuardarCuentasPorCobrar")]
        public async Task<IActionResult> GuardarCuentasPorCobrar([FromBody] CuentasCobrar model)
        {

            if (ModelState.IsValid)
            {
                _context.CuentasCobrar.Add(model);
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

        [HttpGet("ObtenerCuentasPorCobrar/{idusuario}")]
        public async Task<IActionResult> ObtenerCuentasPorCobrar([FromRoute] int idusuario)
        {

            string Sentencia = " exec ObtenerCuentasPorCobrar @idUser";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idUser", idusuario));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

        [HttpGet("eliminarCuentasCobrar/{idcuenta}")]
        public async Task<IActionResult> eliminarCuentasCobrar([FromRoute] int idcuenta)
        {

            string Sentencia = " delete from cuentasCobrar where id = @idcu";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idcu", idcuenta));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

        [HttpPut]
        [Route("actualizarCuentaPagar/{id}")]
        public async Task<IActionResult> actualizarCuentaPagar([FromRoute] int id, [FromBody] CuentasCobrar model)
        {
            if (id != model.Id)
            {
                return BadRequest("No existe la asignacion");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }

    }
}
