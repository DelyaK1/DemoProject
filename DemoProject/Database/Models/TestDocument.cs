using System;
using System.Collections.Generic;

#nullable disable

namespace DemoProject
{
    public partial class TestDocument
    {
        public TestDocument()
        {
            TestFiles = new HashSet<TestFile>();
        }

        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string Extension { get; set; }
        public int TransmittalId { get; set; }
        public string Status { get; set; }

        public virtual TestTransmittal Transmittal { get; set; }
        public virtual ICollection<TestFile> TestFiles { get; set; }
    }
}
