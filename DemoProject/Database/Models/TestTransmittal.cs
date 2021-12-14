using System;
using System.Collections.Generic;

#nullable disable

namespace DemoProject
{
    public partial class TestTransmittal
    {
        public TestTransmittal()
        {
            TestDocuments = new HashSet<TestDocument>();
        }

        public int Id { get; set; }
        public string TransmittalName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Contractor { get; set; }
        public string UserUpload { get; set; }

        public virtual ICollection<TestDocument> TestDocuments { get; set; }
    }
}
