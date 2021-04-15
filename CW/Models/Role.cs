using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class Role
    {
        public Role()
        {
            Workers = new HashSet<Worker>();
        }

        public int RoleId { get; set; }
        public string Role1 { get; set; }
        public int? BaseSalary { get; set; }
        public float? PercentForRoute { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
