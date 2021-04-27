using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        [Display(Name = "Cluent full name")]
        public string ClientFullName => $"{FirstName} {LastName}";

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            DateAdded = DateTime.Now;
            DateUpdated = DateTime.Now;
        }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
