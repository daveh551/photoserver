using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
    public class Photographer : IntEntity
    {
        public string Name { get; set; }
        public string Initials { get; set; }
        public string PhoneNumber { get; set; }
    }
}
