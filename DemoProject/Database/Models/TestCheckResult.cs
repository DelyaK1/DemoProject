using System;
using System.Collections.Generic;

#nullable disable

namespace DemoProject
{
    public partial class TestCheckResult
    {
        public TestCheckResult()
        {
            TestFiles = new HashSet<TestFile>();
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public string Desription { get; set; }
        public int? CheckId { get; set; }

        public virtual ICollection<TestFile> TestFiles { get; set; }
    }
}
