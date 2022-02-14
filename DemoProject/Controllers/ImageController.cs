using DemoProject.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {

        [HttpGet("{id}")]
        public async Task<string> GetImage(int id)
        {
            try
            {
                FileContext context = new FileContext();
                string imagename = context.GetImage(id);
                if (!string.IsNullOrEmpty(imagename))
                    imagename = imagename + ".bmp";

                return imagename;
            }
            catch
            {
                return "";
            }
            
        }
    }
}
