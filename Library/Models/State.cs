using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDLibrary.Models
{
    public class State
    {
        public enum Error
        {
            error,
            warning,
            infomation
        }

        public enum UploadStatus
        {
            error,
            successful
        }
    }
}
