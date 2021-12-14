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
        public async Task<List<DocumentAttributesModel>> GetAttributes()
        {
            FileAttributesContext context = new FileAttributesContext();

            var filesId = context.GetFiles().Select(r => r.Id).ToList();
            return context.GetFileAttributesList(filesId);
        }

        [HttpGet("{id}")]
        public async Task<DocumentAttributesModel> GetAttributes(int id)
        {
            FileAttributesContext context = new FileAttributesContext();
            var docAttributes = context.GetFileAttributes(id);
            return docAttributes;
        }

        [HttpPost]
        public void SaveAttributes()
        { 
            FileAttributesContext context = new FileAttributesContext();

            var filesId = context.GetFiles().Where(r => r.Status != "Successful").Select(r=>r.Id).ToList();
            SaveAttributesProcessor(filesId);
        }

        public static void SaveAttributesProcessor(List<int> filesId)
        {
            FileAttributesContext context = new FileAttributesContext();

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            FileProcessor fileProcessor = new FileProcessor();

            foreach (int id in filesId)
            {
                var bytes = context.GetFileBytes(id);
                if (bytes != null)
                {
                    var model = fileProcessor.GetFileAttributes(bytes);
                    var attrId = context.CreateFileAttributes(model.Result);
                    if (attrId != -1)
                    {
                        context.UpdateFile("Succesful", attrId, id);
                    }
                    else
                    {
                        context.UpdateFile("Error", attrId, id);
                    }
                }

            }
        }

    }
}
