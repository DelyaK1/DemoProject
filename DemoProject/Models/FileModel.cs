using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Models
{
    public class FileModel
    {
        public int pageId {get; set;}
        public int docId {get; set;}
        public string name { get; set; }
        public int pageNumber { get; set; }
        public string type { get; set; }
        public string Status { get; set; }
        public string link { get; set; }
        public byte[] bytes { get; set; }
    }
}
