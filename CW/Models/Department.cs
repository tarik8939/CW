using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class Department
    {
        public Department()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int DeptId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
