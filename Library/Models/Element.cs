using EnigmaSvgCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDLibrary.Models
{
    public class Element
    {
        public string Name { get; set; }
        public Cell Cell { get; set; }
        public object Content { get; set; }
    }
}
