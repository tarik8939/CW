using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class Client
    {
        public Client()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
