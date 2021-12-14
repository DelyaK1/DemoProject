using System;
using System.Collections.Generic;

#nullable disable

namespace DemoProject
{
    public partial class TestAttribute
    {
        public TestAttribute()
        {
            TestFiles = new HashSet<TestFile>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContractorName { get; set; }
        public string FooterName { get; set; }
        public string PapeSize { get; set; }
        public string Scale { get; set; }
        public string StageEn { get; set; }
        public int? Sheet { get; set; }
        public int? TotalSheets { get; set; }
        public string Rev { get; set; }
        public string EngDescription { get; set; }
        public string RusDescription { get; set; }
        public string StageRu { get; set; }
        public string Status { get; set; }
        public string Issue { get; set; }
        public string ClientRev { get; set; }
        public DateTime? Date { get; set; }
        public string PurposeIssue { get; set; }

        public virtual ICollection<TestFile> TestFiles { get; set; }
    }
}
