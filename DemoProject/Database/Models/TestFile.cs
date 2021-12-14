using System;
using System.Collections.Generic;

#nullable disable

namespace DemoProject
{
    public partial class TestFile
    {
        public int Id { get; set; }
        public int PageNumber { get; set; }
        public string Type { get; set; }
        public int DocumentId { get; set; }
        public string Status { get; set; }
        public int ByteId { get; set; }
        public int? AttributesId { get; set; }
        public int? CheckResultsId { get; set; }

        public virtual TestAttribute Attributes { get; set; }
        public virtual TestByte Byte { get; set; }
        public virtual TestCheckResult CheckResults { get; set; }
        public virtual TestDocument Document { get; set; }
    }
}
