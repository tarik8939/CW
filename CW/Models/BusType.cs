using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Type")]
        public string TypeName { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Transport> Transports { get; set; }
    }
}
