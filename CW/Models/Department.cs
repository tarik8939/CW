using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string Fulladdress =>$"{City.City1}: {Address}";

        public virtual City City { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
