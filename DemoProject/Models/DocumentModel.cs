using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Models
{
    public class DocumentModel
    {
        public string Name { get; set; }

        public IFormFile FormFile { get; set; }
    }
}
