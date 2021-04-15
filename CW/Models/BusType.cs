using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class BusType
    {
        public BusType()
        {
            Transports = new HashSet<Transport>();
        }

        public int BusTypeId { get; set; }
        public string TypeName { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Transport> Transports { get; set; }
    }
}
