using DemoProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Interfaces
{
    interface IUploadFilesContext
    {
        public int CreateTransmittal(string user, string contractor);

        public int CreateDocument(int trmId, string document);

        public int CreateFile(int docId, int byteId, string name);

        public int CreateByte(string file);
    }
}
