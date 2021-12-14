using DemoProject.Interfaces;
using DemoProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoProject
{
    public class UploadFilesContext : IUploadFilesContext

    {
        private readonly agcc_rd_testContext _context;

        public UploadFilesContext()
        {
            _context = new agcc_rd_testContext();
        }

        public int CreateTransmittal(string user, string contractor)
        {
            Guid trmName = Guid.NewGuid();
            var trm = new TestTransmittal
            {
                TransmittalName = trmName.ToString(),
                UserUpload = user,
                Contractor = contractor,
                DateCreated = DateTime.Now
            };
            _context.TestTransmittals.Add(trm);
            _context.SaveChanges();

            return trm.Id;
        }

        public int CreateDocument(int trmId, string document)
        {
            var doc = new TestDocument
            {
                DocumentName = document,
                Extension = document.Split('.').Last(),
                TransmittalId = trmId,
                Status = "newborn"
            };
            _context.TestDocuments.Add(doc);
            _context.SaveChanges();

            return doc.Id;
        }

        public int CreateFile(int docId, int byteId, string name)
        {
            var page = int.TryParse(Regex.Match(Path.GetFileNameWithoutExtension(name), "[0-9]+$").Value, out int res) ? res : -1;
            var type = Path.GetExtension(name).Replace(".", string.Empty);
            var file = new TestFile
            {
                DocumentId = docId,
                PageNumber = page,
                Type = type,
                Status = "newborn",
                ByteId = byteId
            };
            _context.TestFiles.Add(file);
            _context.SaveChanges();

            return file.Id;
        }

        public int CreateByte(string file)
        {
            try
            {
                var bytes = File.ReadAllBytes(file);
                var fileBytes = new TestByte
                {
                    Bytes = bytes
                };
                _context.TestBytes.Add(fileBytes);
                _context.SaveChanges();

                return fileBytes.Id;
            }
            catch
            {
                return -1;
            }
        }

        public void DeleteFile(int fileId)
        {
            try
            {
                var file = _context.TestFiles.Find(fileId);
                var bytes = _context.TestBytes.Find(file.ByteId);
                _context.Remove(bytes);
                _context.Remove(file);

                _context.SaveChanges();
            }
            catch
            {

            }
            
        }

        public List<FileModel> GetFilesAsync()
        {
            try
            {
                var files = from f in _context.TestFiles
                                 join d in _context.TestDocuments on f.DocumentId equals d.Id
                                 select new FileModel
                                 {
                                     name = d.DocumentName,
                                     pageNumber = f.PageNumber,
                                     Status = f.Status,
                                     type = f.Type
                                 };

                return files.ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public FileModel GetFilesAsync(int id)
        {
            try
            {
                var files = from f in _context.TestFiles
                            join d in _context.TestDocuments on f.DocumentId equals d.Id
                            where f.Id == id
                            select new FileModel
                            {
                                id = f.Id,
                                name = d.DocumentName,
                                docId = d.Id,
                                pageNumber = f.PageNumber,
                                Status = f.Status,
                                type = f.Type
                            };
                var test = files.FirstOrDefault();
                return files.FirstOrDefault();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

       
        //public void UpdateDocument( int fileId, string status = "Successful")
        //{
        //    try
        //    {
        //        var document = _context.TestDocuments.Where(r => r.Id == _context.TestFiles.FindAsync(fileId).Result.DocumentId).FirstOrDefault();
        //        if(document != null)
        //        {
        //            document.Status = status;
        //            _context.SaveChangesAsync();
        //        }

        //    }
        //    catch { }

        //}
    }
}

