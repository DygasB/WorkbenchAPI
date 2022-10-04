using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace WorkbenchAPI.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration =1200, VaryByQueryKeys = new[] {"fileName"})]
        [AllowAnonymous]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = @$"{rootPath}/PrivateFiles/{fileName}";
            var fileExists = System.IO.File.Exists(filePath);
            if (!fileExists)
            {
                return NotFound();
            }
            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);    
            var fileContents = System.IO.File.ReadAllBytes(filePath);
            //do zwracania plikow sluzy File()
            return File(fileContents, contentType, fileName);
    
        }
        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile file)
        {
            if(file is not null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var fileName = file.FileName; //Name zwraca nazwe parametru
                var fullPath = $"{rootPath}/PrivateFiles/{fileName}";
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok();
            }
            return BadRequest();    
        }

    }
}
