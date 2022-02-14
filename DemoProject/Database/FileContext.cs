using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoProject.Database.Models
{
    public class FileContext
    {
        private readonly agcc_rd_testContext _context;

        public FileContext()
        {
            _context = new agcc_rd_testContext();
        }

        public void UpdateFile(string status, int attrId, int fileId)
        {
            try
            {
                var file = _context.TestFiles.FindAsync(fileId).Result;
                file.Status = status;
                file.AttributesId = attrId;

                _context.SaveChanges();
            }
            catch { }

        }

        public int InsertFile(int docId, string file_path)
        {
            try
            {
                var page = int.TryParse(Regex.Match(Path.GetFileNameWithoutExtension(file_path), "[0-9]+$").Value, out int res) ? res : -1;
                var type = Path.GetExtension(file_path).Replace(".", "");
                var _bytes = File.ReadAllBytes(file_path);

                TestByte bytes = new TestByte
                {
                    Bytes = _bytes
                };
                var file = new TestFile
                {
                    DocumentId = docId,
                    PageNumber = page,
                    Status = "newborn",
                    Byte = bytes,
                    Links = file_path

                };
                _context.TestFiles.Add(file);
                _context.SaveChanges();
                return file.Id;
            }
            catch
            {
                return -1;
            }
            
        }

        public List<TestFile> GetSheets()
        {
            return _context.TestFiles.ToList();
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

        public List<FileModel> GetFiles()
        {
            try
            {
                return _context.TestFiles
                            .Select(f => new FileModel
                            {
                                pageId = f.Id,
                                name = f.Document.DocumentName,
                                docId = f.DocumentId,
                                type = f.Document.Extension,
                                pageNumber = f.PageNumber,
                                Status = f.Status,
                                link = f.Links
                            }).ToList();
            }
            catch
            {
                return null;
            }
        }

        public FileModel GetFile(int pageid)
        {
            var file = _context.TestFiles.Where(r => r.Id == pageid).Select(f => new FileModel
            {
                pageId = f.Id,
                name = f.Document.DocumentName,
                docId = f.DocumentId,
                type = f.Document.Extension,
                pageNumber = f.PageNumber,
                Status = f.Status,
                link = f.Links,
                bytes = f.Byte.Bytes
            })
                .FirstOrDefault();

            return file;
        }

        public string GetImage(int pageid)
        {
            try
            {
                var filemodel = _context.TestFiles.Where(r => r.Id == pageid).Select(f => f.Document.DocumentName).FirstOrDefault();
                return filemodel;
            }
            catch
            {
                return null;
            }            
        }

    }
}
