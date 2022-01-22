using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Models
{
    public class AttributesModel
    {
        public int id {get;set;}
        public int pageId {get;set; }
        public string Document { get; set; }
        public int Page { get; set; }
        public string FileName { get; set; }
        public string ContractorName { get; set; }
        public string FooterName { get; set; }
        public string PapeSize { get; set; }
        public string Scale { get; set; }
        public string StageEn { get; set; }
        public int Sheet { get; set; }
        public int TotalSheets { get; set; }
        public string Rev { get; set; }
        public string EngDescription { get; set; }
        public string RusDescription { get; set; }
        public string StageRu { get; set; }
        public string Status { get; set; }
        public string Issue { get; set; }
        public string ClientRev { get; set; }
        public DateTime Date { get; set; }
        public string PurposeIssue { get; set; }
    }
}
