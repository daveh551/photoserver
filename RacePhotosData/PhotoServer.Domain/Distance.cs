using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
	public class Distance : IntEntity
	{
		[MaxLength(30)]
		public string RaceDistance { get; set; }
	}
}
