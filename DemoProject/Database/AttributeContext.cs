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
    public class AttributeContext
    {
        private readonly agcc_rd_testContext _context;

        public AttributeContext()
        {
            _context = new agcc_rd_testContext();
        }

        public int InsertFileAttributes(RDLibrary.Models.AttributesModel model)
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
            catch
            {
                return -1;
            }
        }

        public List<Models.AttributesModel> GetAttributes(List<int> filesId)
        {
            try
            {
                return filesId.SelectMany(fileId => _context.TestFiles.Where(r => r.Id == fileId)
                .Select(file=> new Models.AttributesModel
                {
                    id = file.Attributes.Id,
                    pageId = fileId,
                    Document = file.Document.DocumentName,
                    Page = file.PageNumber,
                    FileName = file.Attributes.FileName,
                    ContractorName = file.Attributes.ContractorName,
                    FooterName = file.Attributes.FooterName,
                    PapeSize = file.Attributes.PapeSize,
                    Scale = file.Attributes.Scale,
                    StageEn = file.Attributes.StageEn,
                    Sheet = (int)file.Attributes.Sheet,
                    TotalSheets = (int)file.Attributes.TotalSheets,
                    Rev = file.Attributes.Rev,
                    EngDescription = file.Attributes.EngDescription,
                    RusDescription = file.Attributes.RusDescription,
                    StageRu = file.Attributes.StageRu,
                    Status = file.Attributes.Status,
                    Issue = file.Attributes.Issue,
                    ClientRev = file.Attributes.ClientRev,
                    Date = (DateTime)file.Attributes.Date,
                    PurposeIssue = file.Attributes.PurposeIssue
                })).ToList();
            }
            catch
            {
                return null;
            }

        }

        public Models.AttributesModel GetFileAttributes(int fileId)
        {
            try
            {
                return _context.TestFiles.Where(r => r.Id == fileId).Select(file =>
                new Models.AttributesModel
                {
                    id = file.Attributes.Id,
                    pageId = fileId,
                    Document = file.Document.DocumentName,
                    Page = file.PageNumber,
                    FileName = file.Attributes.FileName,
                    ContractorName = file.Attributes.ContractorName,
                    FooterName = file.Attributes.FooterName,
                    PapeSize = file.Attributes.PapeSize,
                    Scale = file.Attributes.Scale,
                    StageEn = file.Attributes.StageEn,
                    Sheet = (int)file.Attributes.Sheet,
                    TotalSheets = (int)file.Attributes.TotalSheets,
                    Rev = file.Attributes.Rev,
                    EngDescription = file.Attributes.EngDescription,
                    RusDescription = file.Attributes.RusDescription,
                    StageRu = file.Attributes.StageRu,
                    Status = file.Attributes.Status,
                    Issue = file.Attributes.Issue,
                    ClientRev = file.Attributes.ClientRev,
                    Date = (DateTime)file.Attributes.Date,
                    PurposeIssue = file.Attributes.PurposeIssue
                })
                .FirstOrDefault();
            }
            catch 
            {
                return new Models.AttributesModel { };
            }

        }
    }
}
