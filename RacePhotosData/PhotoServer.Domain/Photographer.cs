using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
    public class Photographer : IntEntity
    {
		[MaxLength(100)]
        public string Name { get; set; }
		[MaxLength(6)]
        public string Initials { get; set; }
		[MaxLength(20)]
        public string PhoneNumber { get; set; }
    }
}
