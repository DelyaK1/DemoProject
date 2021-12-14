using EnigmaSvgCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RdLibrary.LocalServices
{
    public class DocumentService
    {
        public List<string> SplitDocument(Stream docStream, string docName)
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", docName);
            return Svg.SplitPdfBySinglePage(docStream, System.IO.Path.GetDirectoryName(path), $"{System.IO.Path.GetFileNameWithoutExtension(path)}_page_");
        }

    }
}
