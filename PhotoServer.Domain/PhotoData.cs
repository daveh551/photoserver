using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
    public class PhotoData : GuidEntity
    {
        public string Path { get; set; }
    }
}
