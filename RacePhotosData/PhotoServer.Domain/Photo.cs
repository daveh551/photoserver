using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
    public class Photo : GuidEntity
    {
	    public const string DEFAULTSTATION = "FinishLine";

	    public virtual int  EventId { get; set; }
	    public Event Event { get; set; }
		[MaxLength(50)]
	    public virtual string Station { get; set; }
		[MaxLength(10)]
	    public virtual string Card { get; set; }
	    public virtual int? Sequence { get; set; }
		[MaxLength(512)]
        public virtual string Path { get; set; }
	    public virtual DateTime? TimeStamp { get; set; }
	    public virtual int? Hres { get; set; }
	    public virtual int? Vres { get; set; }
		[MaxLength(10)]
        public string FStop { get; set; }
		[MaxLength(10)]
        public virtual string ShutterSpeed { get; set; }
        public virtual short? ISOspeed { get; set; }
	    public virtual short? FocalLength { get; set; }
	    public virtual Guid? BasedOn { get; set; }
	    public virtual long FileSize { get; set; }
	    public virtual DateTime? LastAccessed { get; set; }
		[MaxLength(100)]
	    public virtual string Server { get; set; }
		[MaxLength(50)]
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
	    public virtual Photographer Photographer { get; set; }



	    public Photo()
	    {
		    
	    }

		public string SetPath()
		{
			Path = string.Format("{0}/{1}/{2}/{3:000}.jpg", Event.ToString(), Station, Card, Sequence);
			return Path;
		}
    }
}
