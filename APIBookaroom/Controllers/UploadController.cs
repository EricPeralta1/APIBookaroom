using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using APIBookaroom.Clases;
using APIBookaroom.Models;

namespace APIBookaroom.Controllers  
{
    public class UploadController : ApiController
    {
        [HttpPost]
        [Route("uploadImage")]
        public async Task<IHttpActionResult> UploadImage([FromBody] UploadImageRequest request)
        {
            try
            {
                var base64Image = request.Base64Image.Trim().Replace("\n", "").Replace("\r", "");

                if (string.IsNullOrEmpty(base64Image))
                {
                    return BadRequest("Base64 image string is empty.");
                }

                var bytes = Convert.FromBase64String(request.Base64Image);
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var imagesFolderPath = Path.Combine(baseDirectory, "Images");

                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                var path = Path.Combine(imagesFolderPath, request.ImageName);

                System.IO.File.WriteAllBytes(path, bytes);

                return Ok("Image uploaded successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error uploading image: {ex.Message}, {request.Base64Image}");
            }
        }
    }
}
