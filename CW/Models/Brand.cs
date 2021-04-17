using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CW.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Transports = new HashSet<Transport>();
        }

        public int BrandId { get; set; }
        [Display(Name = "Brand")]
        public string BrandName { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Transport> Transports { get; set; }
    }
}
