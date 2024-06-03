using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Controllers.files_controllers;
using seinAppBackend.Models;
using System.Data;

namespace ticketsRequerimientosBackend.Controllers
{
    [Route("api/ImageManager")]
    [ApiController]
    public class ImageManagerController : ControllerBase
    {

        readonly private sheinAppContext _context;

        public ImageManagerController(sheinAppContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("crearCarpeta/{nombre}")]
        public async Task<IActionResult> CrearCarpeta([FromForm] IMGmodelClass request, [FromRoute] string nombre)
        {
            string fileModelpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "storage");
            string imagePath = Path.Combine(fileModelpath, nombre);

            try
            {
                // Crear el directorio de almacenamiento si no existe
                if (!Directory.Exists(fileModelpath))
                {
                    Directory.CreateDirectory(fileModelpath);
                }

                // Crear la carpeta específica si no existe
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                // Verificar que el archivo no sea nulo
                if (request.Archivo != null)
                {
                    string fileName = request.Archivo.FileName;
                    string filePath = Path.Combine(imagePath, fileName);

                    // Evitar sobreescribir archivos existentes cambiando el nombre del archivo si ya existe
                    int fileCount = 1;
                    string fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                    string extension = Path.GetExtension(filePath);
                    while (System.IO.File.Exists(filePath))
                    {
                        string tempFileName = $"{fileNameOnly}({fileCount++}){extension}";
                        filePath = Path.Combine(imagePath, tempFileName);
                    }

                    using (FileStream newFile = System.IO.File.Create(filePath))
                    {
                        await request.Archivo.CopyToAsync(newFile);
                        await newFile.FlushAsync();
                    }
                }

                return Ok();
            }
            catch (UnauthorizedAccessException uaEx)
            {
                return StatusCode(403, "No se tienen los permisos necesarios para acceder a la ruta especificada.");
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }



        //private string FormatFileName(string originalFileName)
        //{
        //    string fileNameOnly = Path.GetFileNameWithoutExtension(originalFileName);
        //    string extension = Path.GetExtension(originalFileName);

        //    // Asegurarse de que el nombre del archivo tenga exactamente 10 caracteres
        //    if (fileNameOnly.Length > 10)
        //    {
        //        fileNameOnly = fileNameOnly.Substring(0, 10);
        //    }
        //    else if (fileNameOnly.Length < 10)
        //    {
        //        fileNameOnly = fileNameOnly.PadRight(10, '_'); // Rellenar con guiones bajos si es necesario
        //    }

        //    return fileNameOnly + extension;
        //}



        [HttpGet("ActualizarImgProducto/{url}/{id}")]
        public async Task<IActionResult> ActualizarImgProducto([FromRoute] string url, [FromRoute] int id) {

            string Sentencia = " update producto set urlimagen = @urls where id = @idProd ";
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString)) {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@urls",   url) );
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idProd", id));
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

