using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using seinAppBackend.Models;
using System.Data;

namespace seinAppBackend.Controllers
{
    [Route("api/Usuario")]
    [ApiController]
    public class WebUserController : ControllerBase
    {

        readonly private sheinAppContext _context;

        public WebUserController(sheinAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Usuario userInfo)
        {
            var result = await _context.Usuario.FirstOrDefaultAsync(x => x.Email == userInfo.Email && x.Password == userInfo.Password);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Datos incorrectos");
            }
        }


        [HttpPost]
        [Route("guardarUsuarios")]
        public async Task<IActionResult> guardarUsuarios([FromBody] Usuario model)
        {

            if (ModelState.IsValid)
            {
                _context.Usuario.Add(model);
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

        [HttpPut]
        [Route("actualizarUsuarios/{id}")]
        public async Task<IActionResult> actualizarUsuarios([FromRoute] int id, [FromBody] Usuario model)
        {
            if (id != model.Id)
            {
                return BadRequest("No existe la asignacion");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }

        [HttpGet("obtenerUsuario/{usercrea}")]
        public async Task<IActionResult> obtenerUsuario([FromRoute] string usercrea)
        {
            string Sentencia = " select * from usuario where usercrea = @usercrea and estado = 1 ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@usercrea", usercrea));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido obtener...");
            }

            return Ok(dt);


        }

        [HttpGet("actualizarUsuarioEstado/{estado}/{id}/{idcliente}")]
        public async Task<IActionResult> actualizarUsuarioEstado([FromRoute] int estado, [FromRoute] int id, [FromRoute] int idcliente)
        {
            string Sentencia = " update usuario set estado = @st where usercrea = @usercrea and id = @idcli ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@st", estado));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@usercrea", id));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idcli", idcliente));
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
