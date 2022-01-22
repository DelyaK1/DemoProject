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
    public class DocumentContext

    {
        private readonly agcc_rd_testContext _context;

        public DocumentContext()
        {
            _context = new agcc_rd_testContext();
        }

        public int InsertDocument(string document)
        {
            try
            {
                var type = Path.GetExtension(document).Replace(".", "");
                Guid guid = Guid.NewGuid();
                TestTransmittal transmittal = new TestTransmittal
                {
                    Contractor = "LIN",
                    DateCreated = DateTime.Now,
                    TransmittalName = guid.ToString(),
                    UserUpload = "khabibullinaaf"
                };
                var doc = new TestDocument
                {
                    DocumentName = Path.GetFileNameWithoutExtension(document),
                    Extension = type,
                    Transmittal = transmittal,
                    Status = "newborn"
                };
                _context.TestDocuments.Add(doc);
                _context.SaveChanges();

                return doc.Id;
            }
            catch
            {
                return -1;
            }            
        }

        public void UpdateStatus(int docId)
        {
            try
            { 
                var doc = _context.TestDocuments.Where(r=>r.Id == docId).FirstOrDefault();
                var pages = doc.TestFiles.ToList();
                if(doc != null)
                {
                   if( pages.All(r=>r.Status == "Succesful"))
                    {
                        doc.Status = "Succesful";
                    }
                   else if(pages.Any(r=>r.Status == "Error"))
                    {
                        doc.Status = "Error";
                    }
                    _context.SaveChanges();
                }

            }
            catch { }

        }

        public List<TestDocument> GetDocuments()
        {
            return _context.TestDocuments.ToList();
        }
    }
}

