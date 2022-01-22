using DemoProject.Database.Models;
using DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RDLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AttributesController : Controller
    {
        [HttpGet]
        public async Task<List<AttributesModel>> GetAttributes()
        {
            AttributeContext attributeContext = new AttributeContext();
            FileContext fileContext = new FileContext();
            var filesId = fileContext.GetSheets().Select(r => r.Id).ToList();
            return attributeContext.GetAttributes(filesId);
        }

        [HttpGet("{id}")]
        public async Task<AttributesModel> GetAttributes(int id)
        {
            AttributeContext context = new AttributeContext();
            var docAttributes = context.GetFileAttributes(id);
            return docAttributes;
        }

        [HttpPost("{id}")]
        public void SaveAttributes(int id)
        {
            FileContext fileContext = new FileContext();
            var file = fileContext.GetFile(id);
            var attrId = SaveAttributesProcessor(file.bytes, file.link);
            if (attrId != -1)
            {
                fileContext.UpdateFile("Succesful", attrId, id);
            }
            else
            {
                fileContext.UpdateFile("Error", attrId, id);
            }
        }

        public static int SaveAttributesProcessor(byte[] bytes, string link)
        {
            AttributeContext attributeContext = new AttributeContext();
            FileProcessor fileProcessor = new FileProcessor();
            var model = fileProcessor.GetFileAttributes(bytes, link);
            return attributeContext.InsertFileAttributes(model.Result);
        }
    }
}
