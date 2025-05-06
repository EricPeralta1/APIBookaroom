using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIBookaroom.Clases
{
    public class UploadImageRequest
    {
        public string ImageName { get; set; }
        public string Base64Image { get; set; }
    }
}