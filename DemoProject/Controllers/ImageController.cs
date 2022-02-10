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
    public class ImageController : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<string> GetImage(int id)
        {
            //var docAttributes = context.GetFileAttributes(id);
            return "";
        }
    }
}
