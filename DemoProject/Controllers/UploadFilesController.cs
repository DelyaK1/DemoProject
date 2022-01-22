using DemoProject.Database.Models;
using DemoProject.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RdLibrary.LocalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static RDLibrary.Models.State;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [Consumes("multipart/form-data")]
    [ApiController]

    public class UploadFilesController : Controller
    {
        [HttpGet]
        public List<FileModel> GetFiles()
        {
            FileContext context = new FileContext();

            return context.GetFiles();
        }

        [HttpGet("{id}")]
        public FileModel GetFiles(int id)
        {
            FileContext context = new FileContext();

            return context.GetFile(id);
        }

        [HttpPost]
        public async Task<string> UploadFiles([FromForm] UploadFile document)
        {
            var documentForm = document.File;
            documentForm = HttpContext.Request.Form.Files[0];

            DocumentService service = new DocumentService();
            DocumentContext db = new DocumentContext();
            try
            {
                int docId = db.InsertDocument(documentForm.FileName);
                var files = service.SplitDocument(documentForm.OpenReadStream(), documentForm.FileName);

                foreach (var file in files)
                {
                    FileContext fileContext = new FileContext();
                    var fileId = fileContext.InsertFile(docId, file);
                    //System.IO.File.Delete(file);
                }
                return UploadStatus.successful.ToString();
            }
            catch
            {
                return UploadStatus.error.ToString();
            }
        }

        [HttpDelete("{id}")]
        public async Task<string> DeleteFile(int id)
        {
            DocumentContext db = new DocumentContext();
            try
            {
                //  db.DeleteFile(id);
                return UploadStatus.successful.ToString();
            }
            catch
            {
                return UploadStatus.error.ToString();
            }
        }
    }
}
