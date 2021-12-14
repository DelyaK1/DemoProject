using DemoProject.Models;
using Newtonsoft.Json;
using RDLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoProject
{
    public class FileAttributesContext
    {
        private readonly agcc_rd_testContext _context;

        public FileAttributesContext()
        {
            _context = new agcc_rd_testContext();
        }

        public int CreateFileAttributes(RDLibrary.Models.TestAttribute model)
        {
            try
            {
                TestAttribute attributeTable = new TestAttribute
                {
                    FileName = model.FileName,
                    ClientRev = model.ClientRev,
                    ContractorName = model.ContractorName,
                    Date = model.Date,
                    EngDescription = model.EngDescription,
                    FooterName = model.FooterName,
                    Issue = model.Issue,
                    PapeSize = model.PapeSize,
                    PurposeIssue = model.PurposeIssue,
                    Rev = model.Rev,
                    RusDescription = model.RusDescription,
                    Scale = model.Scale,
                    Sheet = model.Sheet,
                    StageEn = model.StageEn,
                    StageRu = model.StageRu,
                    Status = model.Status,
                    TotalSheets = model.TotalSheets                    
                };

                _context.TestAttributes.Add(attributeTable);
                _context.SaveChanges();

                return attributeTable.Id;
            }
            catch(Exception ex)
            {
                return -1;
            }
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

        public byte[] GetFileBytes(int fileId)
        {
            byte[] bytes = new byte[] { };
            try
            {
                var byteId = _context.TestFiles.Find(fileId).ByteId;
                var test = _context.TestBytes
                .Where(r => r.Id == byteId);
                var test2 = test.ToList()[0];
                var bytesTable = _context.TestBytes
                .Where(r => r.Id == byteId).FirstOrDefault();

                if(bytesTable != null)
                {
                    bytes = bytesTable.Bytes;
                }
               
            }
            catch(Exception ex)
            {
                ;
            }

            return bytes;

        }

        public List<TestDocument> GetFiles()
        {
            return _context.TestDocuments.ToList();
        }

        public DocumentAttributesModel GetFileAttributes(int docId)
        {
            try
            {
                 var attributes = from at in _context.TestAttributes
                                 join f in _context.TestFiles on at.Id equals f.AttributesId
                                 join d in _context.TestDocuments on f.DocumentId equals d.Id
                                 where d.Id == docId
                                 select  new AttributesModel
                                 {
                                     Document = d.DocumentName,
                                     Page = f.PageNumber,
                                     fileId = f.Id,
                                     FileName = at.FileName,
                                     ContractorName = at.ContractorName,
                                     FooterName = at.FooterName,
                                     PapeSize = at.PapeSize,
                                     Scale = at.Scale,
                                     StageEn = at.StageEn,
                                     Sheet = 0,
                                     TotalSheets = 0,
                                     Rev = at.Rev,
                                     EngDescription = at.EngDescription,
                                     RusDescription = at.RusDescription,
                                     StageRu = at.StageRu,
                                     Status = at.Status,
                                     Issue = at.Issue,
                                     ClientRev = at.ClientRev,
                                     Date = new DateTime(),
                                     PurposeIssue = at.PurposeIssue
                                 };

                var res =  new DocumentAttributesModel
                {name =attributes.First().Document, 
                docId =  docId,
                models = attributes.ToArray()
                };

                return res;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public List<DocumentAttributesModel> GetFileAttributesList(List<int> docsId)
        {
            try
            {
                var res = docsId.Select(docId=>new DocumentAttributesModel{ 
                name = _context.TestDocuments.Where(r => r.Id == docId).Select(r => r.DocumentName).FirstOrDefault(),
                docId = docId,
                models = _context.TestDocuments.Join(_context.TestFiles, d => d.Id, f => f.DocumentId, (d, f) => new { d, f })
                    .Join(_context.TestAttributes, df => df.f.AttributesId, at => at.Id, (df, at) => new { df, at })
                    .Where(r => r.df.d.Id == docsId[0])
                    .Select(r => new AttributesModel
                    {
                        Document = r.df.d.DocumentName,
                        Page = r.df.f.PageNumber,
                        fileId = r.df.f.Id,
                        FileName = r.at.FileName,
                        ContractorName = r.at.ContractorName,
                        FooterName = r.at.FooterName,
                        PapeSize = r.at.PapeSize,
                        Scale = r.at.Scale,
                        StageEn = r.at.StageEn,
                        Sheet = (int)r.at.Sheet,
                        TotalSheets = (int)r.at.TotalSheets,
                        Rev = r.at.Rev,
                        EngDescription = r.at.EngDescription,
                        RusDescription = r.at.RusDescription,
                        StageRu = r.at.StageRu,
                        Status = r.at.Status,
                        Issue = r.at.Issue,
                        ClientRev = r.at.ClientRev,
                        Date = (DateTime)r.at.Date,
                        PurposeIssue = r.at.PurposeIssue
                    }).ToArray()
                }).ToList();

                return res;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
