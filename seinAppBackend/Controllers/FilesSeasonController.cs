using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using seinAppBackend.Models;

namespace seinAppBackend.Controllers
{
    [Route("api/FileSeason")]
    [ApiController]
    public class FilesSeasonController : ControllerBase
    {

        readonly private sheinAppContext _context;

        public FilesSeasonController(sheinAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("guardarUrlFile")]
        public async Task<IActionResult> guardarUrlFile([FromBody] FilesSeason model)
        {

            if (ModelState.IsValid)
            {
                _context.FilesSeason.Add(model);
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

    }
}
