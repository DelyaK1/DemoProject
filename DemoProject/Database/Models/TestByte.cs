using System;
using System.Collections.Generic;

#nullable disable

namespace DemoProject
{
    public partial class TestByte
    {
        public TestByte()
        {
            TestFiles = new HashSet<TestFile>();
        }

        public int Id { get; set; }
        public byte[] Bytes { get; set; }

        public virtual ICollection<TestFile> TestFiles { get; set; }
    }
}
