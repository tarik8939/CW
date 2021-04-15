using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int CityId { get; set; }
        public string AddressName { get; set; }

        public virtual City City { get; set; }
    }
}
