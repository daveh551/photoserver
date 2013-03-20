using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.Domain
{
    public abstract class GuidEntity: IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
