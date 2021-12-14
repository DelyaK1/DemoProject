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
                UploadFilesContext context = new UploadFilesContext();

                return context.GetFilesAsync();
             }

            [HttpGet("{id}")]
            public FileModel GetFiles(int id)
            {
                UploadFilesContext context = new UploadFilesContext();

                return context.GetFilesAsync(id);
            }

            [HttpPost]
            public async Task<string> UploadFiles([FromForm] UploadFile document)
            {
               var documentForm = document.File;
               documentForm = HttpContext.Request.Form.Files[0];

                DocumentService service = new DocumentService();
                UploadFilesContext db = new UploadFilesContext();
                try
                {
                    int trmId = db.CreateTransmittal("khabibullinaaf", "LIN");
                    int docId = db.CreateDocument(trmId, documentForm.FileName);

                    var files = service.SplitDocument(documentForm.OpenReadStream(), documentForm.FileName);

                       foreach (var file in files)
                    {
                        int byteId = db.CreateByte(file);
                        var fileId = db.CreateFile(docId, byteId, file);

                        System.IO.File.Delete(file);
                    }
                    return  UploadStatus.successful.ToString();
                }
                catch
                {
                    return  UploadStatus.error.ToString();
                }
            }

            [HttpDelete("{id}")]
            public async Task DeleteActivity(int id)
            {
                UploadFilesContext db = new UploadFilesContext();
                try
                {
                   db.DeleteFile(id);
                }
                catch
                {}
            }


            public static void UploadFilesProcessor(string file, int docId)
            {
            UploadFilesContext db = new UploadFilesContext();
            try
            {
                int byteId = db.CreateByte(file);
                db.CreateFile(docId, byteId, file);

                System.IO.File.Delete(file);
            }
            catch
            {}            
        }
    }
}
